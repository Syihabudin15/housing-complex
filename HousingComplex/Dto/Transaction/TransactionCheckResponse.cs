namespace HousingComplex.Dto.Transaction;

public class TransactionCheckResponse
{
    public string Id { get; set; } = String.Empty;
    public DateTime TransDate { get; set; }
    public string Status { get; set; } = String.Empty;
    public TransactionDetailResponse? TransactionDetailResponse { get; set; }
}