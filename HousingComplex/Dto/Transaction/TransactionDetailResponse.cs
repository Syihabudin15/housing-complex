namespace HousingComplex.Dto.Transaction;

public class TransactionDetailResponse
{
    public string Id { get; set; }
    public string HouseType { get; set; }
    public string Housing { get; set; }
    public long Nominal { get; set; }
    public string Description { get; set; }
    public bool IsPaid { get; set; }
}