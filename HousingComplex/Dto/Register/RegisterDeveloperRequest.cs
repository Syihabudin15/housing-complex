using System.ComponentModel.DataAnnotations;

namespace HousingComplex.Dto.Register;

public class RegisterDeveloperRequest
{
    [Required, EmailAddress] public string Email { get; set; } = String.Empty;
    [Required,StringLength(maximumLength:int.MaxValue, MinimumLength = 6)]
    public string Password { get; set; } = String.Empty;
    [Required] public string Name { get; set; } = String.Empty;
    [Required] public string PhoneNumber { get; set; } = String.Empty;
    [Required] public string Address { get; set; } = String.Empty;
    
}