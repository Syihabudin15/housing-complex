using System.Net;
using System.Security.Claims;
using HousingComplex.Dto;
using HousingComplex.Dto.PaymentGateway;
using HousingComplex.Dto.Transaction;
using HousingComplex.Entities;
using HousingComplex.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HousingComplex.Controllers;

[ApiController]
[Route("api/purchases")]
public class TransactionController : BaseController
{
    private readonly ITransactionService _transactionService;

    public TransactionController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }
    
    [HttpPost]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> CreatePurchase(TransactionRequest request)
    {
        var userEmail = User.FindFirst(ClaimTypes.Email).Value;
        var result = await _transactionService.CreateTransaction(request,userEmail);
        CommonResponse<TransactionResponse> response = new()
        {
            StatusCode = (int)HttpStatusCode.Created,
            Message = "Successfully create transaction",
            Data = result
        };
        return Created("api/purchases", response);
    }
    
    [HttpPost("payments")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> GetPayment(RequestGetPaymentMethod request)
    {
        var result = await _transactionService.GetAllPayment(request);
        CommonResponse<List<PaymentList>> response = new()
        {
            StatusCode = (int)HttpStatusCode.OK,
            Message = "Successfully Get Payment List",
            Data = result
        };
        return Ok(response);
    }
    
    [HttpPost("check-transaction/{id}")]
    [Authorize(Roles = "Developer")]
    public async Task<IActionResult> CheckTransaction(string id)
    {
        var result = await _transactionService.CheckTransaction(id);
        CommonResponse<TransactionCheckResponse> response = new()
        {
            StatusCode = (int)HttpStatusCode.OK,
            Message = "Successfully Check Transaction",
            Data = result
        };
        return Ok(response);
    }
}