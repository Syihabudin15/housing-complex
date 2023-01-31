using System.ComponentModel.DataAnnotations;

namespace HousingComplex.Dto.Register;

public class RegisterAdminRequest
{
    [Required, EmailAddress] public string Email { get; set; } = String.Empty;
    [Required,StringLength(maximumLength:int.MaxValue, MinimumLength = 6)]
    public string Password { get; set; } = String.Empty;
}