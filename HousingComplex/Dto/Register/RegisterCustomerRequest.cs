using System.ComponentModel.DataAnnotations;

namespace HousingComplex.Dto.Register;

public class RegisterCustomerRequest
{
    [Required, EmailAddress] public string Email { get; set; } = String.Empty;
    [Required,StringLength(maximumLength:int.MaxValue, MinimumLength = 6)]
    public string Password { get; set; } = String.Empty;
    [Required] public string FirstName { get; set; } = String.Empty;
    [Required] public string LastName { get; set; } = String.Empty;
    [Required] public string City { get; set; } = String.Empty;
    [Required] public string PostalCode { get; set; } = String.Empty;
    [Required] public string Address { get; set; } = String.Empty;
    [Required] public string PhoneNumber { get; set; } = String.Empty;
}