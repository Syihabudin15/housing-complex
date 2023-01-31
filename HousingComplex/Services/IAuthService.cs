using HousingComplex.Dto.Login;
using HousingComplex.Dto.Register;

namespace HousingComplex.Services;

public interface IAuthService
{
    Task<RegisterResponse> RegisterDeveloper(RegisterDeveloperRequest request);
    Task<RegisterResponse> RegisterCustomer(RegisterCustomerRequest request);
    Task<RegisterResponse> RegisterAdmin(RegisterAdminRequest request);
    Task<LoginResponse> Login(LoginRequest request);
}