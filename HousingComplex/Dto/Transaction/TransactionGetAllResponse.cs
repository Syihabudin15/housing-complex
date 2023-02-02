namespace HousingComplex.Dto.Transaction;

public class TransactionGetAllResponse
{
    public string Id { get; set; }
    public DateTime TransDate { get; set; }
    public TransactionDetailResponse TransactionDetailResponse { get; set; }
}