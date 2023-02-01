namespace HousingComplex.Dto.PaymentGateway;

public class CheckTransactionResponse
{
    public string MerchantOrderId { get; set; } = String.Empty;
    public string Reference { get; set; } = String.Empty;
    public string Amount { get; set; } = String.Empty;
    public string StatusCode { get; set; } = String.Empty;
    public string StatusMessage { get; set; } = String.Empty;
}