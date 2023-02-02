namespace HousingComplex.Dto.Transaction;

public class TransactionRequest
{
    public string HouseTypeId { get; set; } = String.Empty;
    public string Description { get; set; } = String.Empty;
    public string PaymentMethod { get; set; } = String.Empty;
    public long NominalTransaction { get; set; }
}