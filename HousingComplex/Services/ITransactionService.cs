using HousingComplex.Dto.PaymentGateway;
using HousingComplex.Dto.Transaction;
using HousingComplex.Entities;

namespace HousingComplex.Services;

public interface ITransactionService
{
    Task<TransactionResponse> CreateTransaction(TransactionRequest request, string email);
    Task<List<PaymentList>> GetAllPayment(RequestGetPaymentMethod request);
    Task<TransactionCheckResponse> CheckTransaction(string id);
}