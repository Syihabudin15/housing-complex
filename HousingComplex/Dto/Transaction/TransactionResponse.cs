﻿using HousingComplex.Entities;

namespace HousingComplex.Dto.Transaction;

public class TransactionResponse
{
    public string Id { get; set; }
    public DateTime TransDate { get; set; }
    public string VirtualAccountNumber { get; set; }
    public TransactionDetailResponse TransactionDetailResponse { get; set; }
}