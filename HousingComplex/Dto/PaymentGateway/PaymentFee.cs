using Newtonsoft.Json;

namespace HousingComplex.Dto.PaymentGateway;

public class PaymentFee
{
    [JsonProperty("paymentFee")]
    public List<PaymentList>? PaymentLists { get; set; }
}