namespace HousingComplex.Dto.PaymentGateway;

public class RequestTransactionResponse
{
    public string MerchantCode { get; set; } = string.Empty;
    public string Reference { get; set; } = string.Empty;
    public string PaymentUrl { get; set; } = string.Empty;
    public string VaNumber { get; set; } = string.Empty;
    public string QrString { get; set; } = string.Empty;
    public string Amount { get; set; } = string.Empty;
    public string StatusCode { get; set; } = string.Empty;
    public string StatusMessage { get; set; } = string.Empty;
    public string OrderId { get; set; } = string.Empty;
}