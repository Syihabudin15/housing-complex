using System.Security.Cryptography;
using System.Text;
using HousingComplex.Dto.PaymentGateway;
using HousingComplex.Dto.Transaction;
using HousingComplex.DTOs;
using HousingComplex.Entities;
using HousingComplex.Exceptions;
using HousingComplex.Repositories;
using HousingComplex.Utils;
using Newtonsoft.Json;

namespace HousingComplex.Services.Impl;

public class TransactionService : ITransactionService
{
    private readonly IRepository<Transaction> _repository;
    private readonly IPersistence _persistence;
    private readonly ICustomerService _customerService;
    private readonly IHouseTypeService _houseTypeService;
    private readonly string _merchantCode = "D11609";
    private readonly string _urlRequestPayment = "https://sandbox.duitku.com/webapi/api/merchant/v2/inquiry";
    private readonly string _urlGetPayment = "https://sandbox.duitku.com/webapi/api/merchant/paymentmethod/getpaymentmethod";
    private readonly string _urlCheckTransaction = "https://sandbox.duitku.com/webapi/api/merchant/transactionStatus";
    private readonly string _apikey = "aecf5ce196890d6bfe994520de94b944";


    public TransactionService(IRepository<Transaction> repository, IPersistence persistence,
        ICustomerService customerService, IHouseTypeService houseTypeService)
    {
        _repository = repository;
        _persistence = persistence;
        _customerService = customerService;
        _houseTypeService = houseTypeService;
    }

    public async Task<TransactionRequestResponse> CreateTransaction(TransactionRequest request, string email)
    {
        var customer = await _customerService.GetForTransaction(email);
        var houseType = await _houseTypeService.GetForTransaction(request.HouseTypeId);
        if(customer.Meet == null) throw new MeetingStatusNotTrueException("The customer must first meet with the developer");
        var meet = customer.Meet.ToList()[customer.Meet.Count - 1];
        // var hs= customer.Meet.Select(meet => meet.HousingId.Equals(houseType.HousingId));
        if(!meet.IsMeet)
            throw new MeetingStatusNotTrueException("The customer must first meet with the developer");

        var requestTransactionDuitku = await RequestTransactionDuitku(customer, houseType, request);

        var transactionResult = await _persistence.ExecuteTransactionAsync(async () =>
        {
            var transaction = new Transaction
            {
                TransDate = DateTime.Now,
                Customer = customer,
                TransactionDetail = new TransactionDetail
                {
                    HouseTypeId = houseType.Id,
                    HousingId = houseType.HousingId,
                    ReferencePg = requestTransactionDuitku.Reference,
                    Nominal = request.NominalTransaction,
                    Description = request.Description,
                    IsPaid = false,
                    OrderId = requestTransactionDuitku.OrderId
                }
            };
            var savedTransaction = await _repository.Save(transaction);
            await _persistence.SaveChangesAsync();
            return new TransactionRequestResponse()
            {
                Id = savedTransaction.Id.ToString(),
                TransDate = savedTransaction.TransDate,
                VirtualAccountNumber = requestTransactionDuitku.VaNumber,
                TransactionDetailResponse = TransactionDetailResponse(transaction.TransactionDetail)
            };
        });

        return transactionResult;
    }

    public async Task<List<PaymentList>> GetAllPayment(RequestGetPaymentMethod request)
    {
        var requestPaymentsDuitku = await RequestPaymentsDuitku(request.Amount);
        return requestPaymentsDuitku;
    }

    public async Task<PageResponse<TransactionGetAllResponse>> GetAllTransaction(int page, int size, string email)
    {
        var transaction = await _repository.FindAll(transaction => 
                transaction.TransactionDetail.Housing.Developer.UserCredential.Email.Equals(email), page, size,
        new[]
        {
            "TransactionDetail.Housing.Developer.UserCredential",
            "TransactionDetail.HouseType"
        });

        var response = transaction.Select(transaction => new TransactionGetAllResponse
        {
            Id = transaction.Id.ToString(),
            TransDate = transaction.TransDate,
            TransactionDetailResponse = TransactionDetailResponse(transaction.TransactionDetail)
        }).ToList();
        
        var totalPage = (int)Math.Ceiling((await _repository.Count(transaction => 
            transaction.TransactionDetail.Housing.Developer.UserCredential.Email.Equals(email),new []
        {
            "TransactionDetail.Housing.Developer.UserCredential",
            "TransactionDetail.HouseType"
        })) / (decimal)size);
        PageResponse<TransactionGetAllResponse> result = new()
        {
            Content = response,
            TotalPages = totalPage,
            TotalElement = transaction.Count()
        };

        return result;
    }

    public async Task<TransactionCheckResponse> CheckTransaction(string id)
    {
        var transaction = await _repository.Find(transaction => transaction.Id.Equals(Guid.Parse(id)),
            new[] { "TransactionDetail.Housing","TransactionDetail.HouseType" });
        if (transaction is null) throw new NotFoundException("Transaction Not Found");
        var requestCheckTransaction = await RequestCheckTransactionDuitku(transaction);
        switch (requestCheckTransaction.StatusCode)
        {
            case "01" or "02":
                return new TransactionCheckResponse
                {
                    Id = transaction.Id.ToString(),
                    TransDate = transaction.TransDate,
                    Status = requestCheckTransaction.StatusMessage,
                    TransactionDetailResponse = TransactionDetailResponse(transaction.TransactionDetail)
                };
            case "00":
                transaction.TransactionDetail.IsPaid = true;
                transaction.TransactionDetail.HouseType.StockUnit -= 1;
                await _persistence.SaveChangesAsync();
                return new TransactionCheckResponse
                {
                    Id = transaction.Id.ToString(),
                    TransDate = transaction.TransDate,
                    Status = requestCheckTransaction.StatusMessage,
                    TransactionDetailResponse = TransactionDetailResponse(transaction.TransactionDetail)
                };
        }
        return null;
    }

    private async Task<CheckTransactionResponse> RequestCheckTransactionDuitku(Transaction request)
    {
        var signature = $"{_merchantCode}{request.TransactionDetail.OrderId}{_apikey}";
        var signatureHash = GenerateMd5(signature);
        Object obj = new
        {
            merchantcode = _merchantCode,
            merchantOrderId = request.TransactionDetail.OrderId,
            signature = signatureHash
        };
        
        var json = JsonConvert.SerializeObject(obj);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        using var client = new HttpClient();
        var response = await client.PostAsync(_urlCheckTransaction, data);
        var result = await response.Content.ReadAsStringAsync();
        var resultResponse = JsonConvert.DeserializeObject<CheckTransactionResponse>(result);
    
        return resultResponse;
    }

    private async Task<List<PaymentList>> RequestPaymentsDuitku(int amount)
    {
        var time = DateTime.Now.AddDays(1);
        var signature = $"{_merchantCode}{amount}{time}{_apikey}";
        var signatureHash = SHA256(signature);
        Object obj = new
        {
            merchantcode = _merchantCode,
            amount = amount,
            datetime = time.ToString("MM/dd/yyyy HH:mm:ss"),
            signature = signatureHash
        };
        
        var json = JsonConvert.SerializeObject(obj);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        using var client = new HttpClient();
        var response = await client.PostAsync(_urlGetPayment, data);
        var result = await response.Content.ReadAsStringAsync();
        var responseObj = JsonConvert.DeserializeObject<PaymentFee>(result);
        var responses = responseObj.PaymentLists.Select(list => new PaymentList
        {
            paymentMethod = list.paymentMethod,
            paymentName = list.paymentName,
            paymentImage = list.paymentImage,
            totalFee = list.totalFee
        }).ToList();
        return responses;
    }

    private async Task<RequestTransactionResponse> RequestTransactionDuitku(Customer customer, HouseType houseType,TransactionRequest request)
    {
        var randomCode = RandomCode.GenerateRandomCode();
        var signature = $"{_merchantCode}{randomCode}{request.NominalTransaction}{_apikey}";
        var signatureHash = GenerateMd5(signature); 
        
        Object obj = new
        {
            merchantCode = _merchantCode,
            paymentAmount = request.NominalTransaction,
            paymentMethod = request.PaymentMethod,
            merchantOrderId = randomCode,
            productDetails = houseType.Name,
            customerVaName = customer.FirstName + " " +customer.LastName,
            email = customer.UserCredential.Email,
            phoneNumber = customer.PhoneNumber,
            callbackUrl = "{{mockUrl}}/callback",
            returnUrl = "{{mockUrl}}/return",
            signature = signatureHash,
            expiryPeriod = 15
        };
        
        var json = JsonConvert.SerializeObject(obj);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        using var client = new HttpClient();
        var response = await client.PostAsync(_urlRequestPayment, data);
        var result = await response.Content.ReadAsStringAsync();

        var resultResponse = JsonConvert.DeserializeObject<RequestTransactionResponse>(result);
        resultResponse.OrderId = randomCode;
        

        return resultResponse;
    }

    private string GenerateMd5(string signature)
    {
        var hashValue = string.Join("", MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(signature)).Select(s => s.ToString("x2")));
        return hashValue;
    }
    private string SHA256(string clearText)
    {
        var sb = new StringBuilder();
        var bytes = Encoding.UTF8.GetBytes(clearText);
        var algo = HashAlgorithm.Create(nameof(SHA256));
        var hash = algo!.ComputeHash(bytes);
        foreach (var byt in hash)
            sb.Append(byt.ToString("x2"));
        return sb.ToString();
    }

    private TransactionDetailResponse TransactionDetailResponse(TransactionDetail request)
    {
        var transactionDetailResponse = new TransactionDetailResponse
        {
            Id = request.Id.ToString(),
            HouseType = request.HouseType.Name,
            Housing = request.Housing.Name,
            Nominal = request.Nominal,
            Description = request.Description,
            IsPaid = request.IsPaid
        };
        return transactionDetailResponse;
    }
}