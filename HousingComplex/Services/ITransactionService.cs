using HousingComplex.Dto.PaymentGateway;
using HousingComplex.Dto.Transaction;
using HousingComplex.DTOs;
using HousingComplex.Entities;

namespace HousingComplex.Services;

public interface ITransactionService
{
    Task<TransactionRequestResponse> CreateTransaction(TransactionRequest request, string email);
    Task<List<PaymentList>> GetAllPayment(RequestGetPaymentMethod request);
    Task<PageResponse<TransactionGetAllResponse>> GetAllTransaction(int page, int size, string email);
    Task<TransactionCheckResponse> CheckTransaction(string id);
}