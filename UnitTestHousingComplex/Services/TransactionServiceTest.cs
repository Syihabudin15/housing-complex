using System.Linq.Expressions;
using HousingComplex.Dto.PaymentGateway;
using HousingComplex.Dto.Transaction;
using HousingComplex.Entities;
using HousingComplex.Exceptions;
using HousingComplex.Repositories;
using HousingComplex.Services;
using HousingComplex.Services.Impl;
using Moq;

namespace UnitTestHousingComplex.Services;

public class TransactionServiceTest
{
    private readonly Mock<IRepository<Transaction>> _mockRepository;
    private readonly Mock<IPersistence> _mockPersistence;
    private readonly Mock<ICustomerService> _mockCustomerService;
    private readonly Mock<IHouseTypeService> _mockHouseTypeService;
    private readonly ITransactionService _transactionService;

    public TransactionServiceTest()
    {
        _mockRepository = new Mock<IRepository<Transaction>>();
        _mockPersistence = new Mock<IPersistence>();
        _mockCustomerService = new Mock<ICustomerService>();
        _mockHouseTypeService = new Mock<IHouseTypeService>();
        _transactionService = new TransactionService(_mockRepository.Object, _mockPersistence.Object,
            _mockCustomerService.Object, _mockHouseTypeService.Object);
    }

    [Fact]
    public async Task Should_Return_TransactionRequestResponse_When_CreateTransacioan()
    {
        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            FirstName = "Achmad",
            LastName = "Fikri",
            City = "Bogor",
            PostalCode = "16913",
            Address = "Cibinong",
            PhoneNumber = "091283",
            UserCredentialId = Guid.NewGuid(),
            UserCredential = new UserCredential
            {
                Id = Guid.NewGuid(),
                Email = "achmad@gmail.com",
                Password = "password",
                RoleId = Guid.NewGuid(),
                Role = null
            },
            Meet = new List<Meet>()
            {
                new Meet
                {
                    Id = Guid.NewGuid(),
                    MeetDate = "2023-02-06",
                    IsMeet = true,
                    HousingId = default,
                    CustomerId = Guid.NewGuid(),
                    Housing = new Housing(),
                    Customer = new Customer()
                }
            }
        };

        var houseType = new HouseType
        {
            Id = Guid.NewGuid(),
            Name = "Type 30/72",
            SpesificationId = Guid.NewGuid(),
            Description = "Dengan Kolam Renang",
            HousingId = Guid.NewGuid(),
            Price = 100000000,
            StockUnit = 20,
            ImageHouseTypeId = Guid.NewGuid(),
            Spesification = new Spesification(),
            Housing = new Housing
            {
                Id = Guid.NewGuid(),
                Name = "Emerald Cilebut",
                DeveloperId = Guid.NewGuid(),
                Address = "Cilebut",
                OpenTime = "weekday",
                City = "bogor",
                Meets = new List<Meet>(),
                Developer = new Developer()
            },
            ImageHouseType = new ImageHouseType()
        };

        var request = new TransactionRequest
        {
            HouseTypeId = Guid.NewGuid().ToString(),
            Description = "Dp",
            PaymentMethod = "M1",
            NominalTransaction = 1000000
        };

        var transactionRequestResponse = new TransactionRequestResponse
        {
            Id = Guid.NewGuid().ToString(),
            TransDate = DateTime.Now,
            VirtualAccountNumber = "83201983129",
            TransactionDetailResponse = new TransactionDetailResponse()
        };

        _mockCustomerService.Setup(cust => cust.GetForTransaction(It.IsAny<string>()))
            .ReturnsAsync(customer);
        _mockHouseTypeService.Setup(house => house.GetForTransaction(It.IsAny<string>()))
            .ReturnsAsync(houseType);
        _mockPersistence.Setup(per => per.ExecuteTransactionAsync(It.IsAny<Func<Task<TransactionRequestResponse>>>()))
            .Callback<Func<Task<TransactionRequestResponse>>>(func => func())
            .ReturnsAsync(transactionRequestResponse);

        var result = await _transactionService.CreateTransaction(request, "achmad@gmail.com");

        Assert.Equal(transactionRequestResponse.Id, result.Id);
        Assert.Equal(transactionRequestResponse.TransDate, result.TransDate);
        Assert.Equal(transactionRequestResponse.VirtualAccountNumber, result.VirtualAccountNumber);
        Assert.Equal(transactionRequestResponse.TransactionDetailResponse, result.TransactionDetailResponse);
    }

    [Fact]
    public async Task Should_ThrowMeetingStatusNotTrueException_When_CreateTransactionIsMeetNull()
    {
        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            FirstName = "Achmad",
            LastName = "Fikri",
            City = "Bogor",
            PostalCode = "16913",
            Address = "Cibinong",
            PhoneNumber = "091283",
            UserCredentialId = Guid.NewGuid(),
            UserCredential = new UserCredential
            {
                Id = Guid.NewGuid(),
                Email = "achmad@gmail.com",
                Password = "password",
                RoleId = Guid.NewGuid(),
                Role = null
            },
            Meet = null
        };

        var houseType = new HouseType
        {
            Id = Guid.NewGuid(),
            Name = "Type 30/72",
            SpesificationId = Guid.NewGuid(),
            Description = "Dengan Kolam Renang",
            HousingId = Guid.NewGuid(),
            Price = 100000000,
            StockUnit = 20,
            ImageHouseTypeId = Guid.NewGuid(),
            Spesification = new Spesification(),
            Housing = new Housing
            {
                Id = Guid.NewGuid(),
                Name = "Emerald Cilebut",
                DeveloperId = Guid.NewGuid(),
                Address = "Cilebut",
                OpenTime = "weekday",
                City = "bogor",
                Meets = new List<Meet>(),
                Developer = new Developer()
            },
            ImageHouseType = new ImageHouseType()
        };

        var request = new TransactionRequest
        {
            HouseTypeId = Guid.NewGuid().ToString(),
            Description = "Dp",
            PaymentMethod = "M1",
            NominalTransaction = 1000000
        };

        var transactionRequestResponse = new TransactionRequestResponse
        {
            Id = Guid.NewGuid().ToString(),
            TransDate = DateTime.Now,
            VirtualAccountNumber = "83201983129",
            TransactionDetailResponse = new TransactionDetailResponse()
        };

        _mockCustomerService.Setup(cust => cust.GetForTransaction(It.IsAny<string>()))
            .ReturnsAsync(customer);
        _mockHouseTypeService.Setup(house => house.GetForTransaction(It.IsAny<string>()))
            .ReturnsAsync(houseType);
        _mockPersistence.Setup(per => per.ExecuteTransactionAsync(It.IsAny<Func<Task<TransactionRequestResponse>>>()))
            .Callback<Func<Task<TransactionRequestResponse>>>(func => func())
            .ReturnsAsync(transactionRequestResponse);

        await Assert.ThrowsAsync<MeetingStatusNotTrueException>(() =>
            _transactionService.CreateTransaction(request, "achmad@gmail.com"));
    }

    [Fact]
    public async Task Should_ThrowMeetingStatusNotTrueException_When_CreateTransactionIsMeetFalse()
    {
        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            FirstName = "Achmad",
            LastName = "Fikri",
            City = "Bogor",
            PostalCode = "16913",
            Address = "Cibinong",
            PhoneNumber = "091283",
            UserCredentialId = Guid.NewGuid(),
            UserCredential = new UserCredential
            {
                Id = Guid.NewGuid(),
                Email = "achmad@gmail.com",
                Password = "password",
                RoleId = Guid.NewGuid(),
                Role = null
            },
            Meet = new List<Meet>()
            {
                new Meet
                {
                    Id = Guid.NewGuid(),
                    MeetDate = "2023-02-06",
                    IsMeet = false,
                    HousingId = default,
                    CustomerId = Guid.NewGuid(),
                    Housing = new Housing(),
                    Customer = new Customer()
                }
            }
        };

        var houseType = new HouseType
        {
            Id = Guid.NewGuid(),
            Name = "Type 30/72",
            SpesificationId = Guid.NewGuid(),
            Description = "Dengan Kolam Renang",
            HousingId = Guid.NewGuid(),
            Price = 100000000,
            StockUnit = 20,
            ImageHouseTypeId = Guid.NewGuid(),
            Spesification = new Spesification(),
            Housing = new Housing
            {
                Id = Guid.NewGuid(),
                Name = "Emerald Cilebut",
                DeveloperId = Guid.NewGuid(),
                Address = "Cilebut",
                OpenTime = "weekday",
                City = "bogor",
                Meets = new List<Meet>(),
                Developer = new Developer()
            },
            ImageHouseType = new ImageHouseType()
        };

        var request = new TransactionRequest
        {
            HouseTypeId = Guid.NewGuid().ToString(),
            Description = "Dp",
            PaymentMethod = "M1",
            NominalTransaction = 1000000
        };

        _mockCustomerService.Setup(cust => cust.GetForTransaction(It.IsAny<string>()))
            .ReturnsAsync(customer);
        _mockHouseTypeService.Setup(house => house.GetForTransaction(It.IsAny<string>()))
            .ReturnsAsync(houseType);
        await Assert.ThrowsAsync<MeetingStatusNotTrueException>(() =>
            _transactionService.CreateTransaction(request, "achmad@gmail.com"));
    }

    [Fact]
    public async Task Should_ReturnListPaymentList_When_GetAllPayment()
    {
        var requestGetPayment = new RequestGetPaymentMethod
        {
            Amount = 1000000
        };
        var result = await _transactionService.GetAllPayment(requestGetPayment);

        Assert.NotNull(result);
    }

    [Fact]
    public async Task Should_ReturnPageResponseTransactionGetAllResponse_When_GetAllTransaction()
    {
        var transaction = new List<Transaction>()
        {
            new Transaction()
            {
                Id = Guid.NewGuid(),
                TransDate = DateTime.Now,
                CustomerId = Guid.NewGuid(),
                Customer = new Customer(),
                TransactionDetail = new TransactionDetail
                {
                    Id = Guid.NewGuid(),
                    HouseTypeId = Guid.NewGuid(),
                    TransactionId = Guid.NewGuid(),
                    HousingId = Guid.NewGuid(),
                    ReferencePg = "jad0s8d0hsa8i9",
                    Nominal = 100000,
                    Description = "Dp",
                    IsPaid = false,
                    OrderId = "jd09as",
                    HouseType = new HouseType
                    {
                        Id = Guid.NewGuid(),
                        Name = "Type 30/72",
                        SpesificationId = Guid.NewGuid(),
                        Description = "Kolam renang",
                        HousingId = Guid.NewGuid(),
                        Price = 100000000,
                        StockUnit = 20,
                        ImageHouseTypeId = Guid.NewGuid(),
                        Spesification = null,
                        Housing = null,
                        ImageHouseType = null
                    },
                    Transaction = null,
                    Housing = new Housing
                    {
                        Id = Guid.NewGuid(),
                        Name = "Emerald Cilebut",
                        DeveloperId = Guid.NewGuid(),
                        Address = "Cilebut",
                        OpenTime = "weekday",
                        City = "Bogor",
                        Meets = null,
                        Developer = new Developer
                        {
                            Id = Guid.NewGuid(),
                            Name = "Emerald",
                            PhoneNumber = "0980970",
                            UserCredentialId = Guid.NewGuid(),
                            Address = "Bogor",
                            UserCredential = new UserCredential
                            {
                                Id = Guid.NewGuid(),
                                Email = "dev@gmail.com",
                                Password = "password",
                                RoleId = Guid.NewGuid(),
                                Role = null
                            },
                            Housing = null
                        }
                    }
                }
            }
        };

        _mockRepository.Setup(repo => repo.FindAll(It.IsAny<Expression<Func<Transaction, bool>>>(),
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<string[]>())).ReturnsAsync(transaction);

        var result = await _transactionService.GetAllTransaction(1, 5, "dev@gmail.com");
        
        Assert.Equal(transaction[0].TransDate,result.Content[0].TransDate);
        Assert.Equal(transaction[0].Id.ToString(),result.Content[0].Id);
        Assert.NotNull(result.Content[0].TransactionDetailResponse);
        Assert.Equal(transaction.Count, result.Content.Count);
        Assert.NotNull(transaction[0].Customer);
        Assert.NotNull(transaction[0].CustomerId);
        Assert.NotNull(transaction[0].TransactionDetail.HouseTypeId);
        Assert.NotNull(transaction[0].TransactionDetail.TransactionId);
        Assert.NotNull(transaction[0].TransactionDetail.HousingId);
        Assert.NotNull(transaction[0].TransactionDetail.ReferencePg);
        Assert.NotNull(transaction[0].TransactionDetail.OrderId);
        Assert.Null(transaction[0].TransactionDetail.Transaction);

        Assert.NotNull(transaction[0].TransactionDetail.HouseType.SpesificationId);
        Assert.NotNull(transaction[0].TransactionDetail.HouseType.Description);
        Assert.NotNull(transaction[0].TransactionDetail.HouseType.Price);
        Assert.NotNull(transaction[0].TransactionDetail.HouseType.StockUnit);
        Assert.NotNull(transaction[0].TransactionDetail.HouseType.ImageHouseTypeId);
        Assert.Null(transaction[0].TransactionDetail.HouseType.Spesification);
        Assert.Null(transaction[0].TransactionDetail.HouseType.Housing);
        Assert.Null(transaction[0].TransactionDetail.HouseType.ImageHouseType);
    }

    [Fact]
    public async Task Should_TransactionCheckResponse_When_CheckTransactionSuccess()
    {
        var transaction = new Transaction()
        {
            Id = Guid.NewGuid(),
            TransDate = DateTime.Now,
            CustomerId = Guid.NewGuid(),
            Customer = new Customer(),
            TransactionDetail = new TransactionDetail
            {
                Id = Guid.NewGuid(),
                HouseTypeId = Guid.NewGuid(),
                TransactionId = Guid.NewGuid(),
                HousingId = Guid.NewGuid(),
                ReferencePg = "jad0s8d0hsa8i9",
                Nominal = 100000,
                Description = "Dp",
                IsPaid = false,
                OrderId = "je5PYLVgtydFo6d",
                HouseType = new HouseType
                {
                    Id = Guid.NewGuid(),
                    Name = "Type 30/72",
                    SpesificationId = Guid.NewGuid(),
                    Description = "Kolam renang",
                    HousingId = Guid.NewGuid(),
                    Price = 100000000,
                    StockUnit = 20,
                    ImageHouseTypeId = Guid.NewGuid(),
                    Spesification = null,
                    Housing = null,
                    ImageHouseType = null
                },
                Transaction = null,
                Housing = new Housing
                {
                    Id = Guid.NewGuid(),
                    Name = "Emerald Cilebut",
                    DeveloperId = Guid.NewGuid(),
                    Address = "Cilebut",
                    OpenTime = "weekday",
                    City = "Bogor",
                    Meets = null,
                    Developer = null
                }
            }
        };
        _mockRepository.Setup(repo => repo.Find(It.IsAny<Expression<Func<Transaction, bool>>>(),
            It.IsAny<string[]>())).ReturnsAsync(transaction);

        var result = await _transactionService.CheckTransaction("dwasadasdad");
        Assert.Equal(transaction.Id.ToString(),result.Id);
        Assert.Equal(transaction.TransDate,result.TransDate);
        Assert.NotNull(result.Status);
        Assert.NotNull(result.TransactionDetailResponse);
        Assert.Equal(transaction.TransactionDetail.HouseType.Name,result.TransactionDetailResponse.HouseType);
        Assert.Equal(transaction.TransactionDetail.Id.ToString(),result.TransactionDetailResponse.Id);
        Assert.Equal(transaction.TransactionDetail.Housing.Name,result.TransactionDetailResponse.Housing);
        Assert.Equal(transaction.TransactionDetail.Nominal,result.TransactionDetailResponse.Nominal);
        Assert.Equal(transaction.TransactionDetail.Description,result.TransactionDetailResponse.Description);
        Assert.Equal(transaction.TransactionDetail.IsPaid,result.TransactionDetailResponse.IsPaid);
        
    }

    [Fact]
    public async Task Should_TransactionCheckResponse_When_CheckTransactionExpiredOrPending()
    {
        var transaction = new Transaction()
        {
            Id = Guid.NewGuid(),
            TransDate = DateTime.Now,
            CustomerId = Guid.NewGuid(),
            Customer = new Customer(),
            TransactionDetail = new TransactionDetail
            {
                Id = Guid.NewGuid(),
                HouseTypeId = Guid.NewGuid(),
                TransactionId = Guid.NewGuid(),
                HousingId = Guid.NewGuid(),
                ReferencePg = "jad0s8d0hsa8i9",
                Nominal = 100000,
                Description = "Dp",
                IsPaid = false,
                OrderId = "nrzqITTKJOdtDGu",
                HouseType = new HouseType
                {
                    Id = Guid.NewGuid(),
                    Name = "Type 30/72",
                    SpesificationId = Guid.NewGuid(),
                    Description = "Kolam renang",
                    HousingId = Guid.NewGuid(),
                    Price = 100000000,
                    StockUnit = 20,
                    ImageHouseTypeId = Guid.NewGuid(),
                    Spesification = null,
                    Housing = null,
                    ImageHouseType = null
                },
                Transaction = null,
                Housing = new Housing
                {
                    Id = Guid.NewGuid(),
                    Name = "Emerald Cilebut",
                    DeveloperId = Guid.NewGuid(),
                    Address = "Cilebut",
                    OpenTime = "weekday",
                    City = "Bogor",
                    Meets = null,
                    Developer = null
                }
            }
        };
        _mockRepository.Setup(repo => repo.Find(It.IsAny<Expression<Func<Transaction, bool>>>(),
            It.IsAny<string[]>())).ReturnsAsync(transaction);
        var result = await _transactionService.CheckTransaction("dwasadasdad");
        Assert.Equal(transaction.Id.ToString(),result.Id);
        Assert.Equal(transaction.TransDate,result.TransDate);
        Assert.NotNull(result.Status);
        Assert.NotNull(result.TransactionDetailResponse);
        Assert.Equal(transaction.TransactionDetail.HouseType.Name,result.TransactionDetailResponse.HouseType);
        Assert.Equal(transaction.TransactionDetail.Id.ToString(),result.TransactionDetailResponse.Id);
        Assert.Equal(transaction.TransactionDetail.Housing.Name,result.TransactionDetailResponse.Housing);
        Assert.Equal(transaction.TransactionDetail.Nominal,result.TransactionDetailResponse.Nominal);
        Assert.Equal(transaction.TransactionDetail.Description,result.TransactionDetailResponse.Description);
        Assert.Equal(transaction.TransactionDetail.IsPaid,result.TransactionDetailResponse.IsPaid);
    }
}