using HousingComplex.Dto.Login;
using HousingComplex.Dto.Register;
using HousingComplex.Entities;
using HousingComplex.Exceptions;
using HousingComplex.Repositories;
using HousingComplex.Security;

namespace HousingComplex.Services.Impl;

public class AuthService : IAuthService
{
    private readonly IRepository<UserCredential> _repository;
    private readonly IPersistence _persistence;
    private readonly IRoleService _roleService;
    private readonly IDeveloperService _developerService;
    private readonly ICustomerService _customerService;
    private readonly IJwtUtils _jwtUtils;

    public AuthService(IRepository<UserCredential> repository, IPersistence persistence, IRoleService roleService, IDeveloperService developerService, ICustomerService customerService, IJwtUtils jwtUtils)
    {
        _repository = repository;
        _persistence = persistence;
        _roleService = roleService;
        _developerService = developerService;
        _customerService = customerService;
        _jwtUtils = jwtUtils;
    }

    public async Task<RegisterResponse> RegisterDeveloper(RegisterDeveloperRequest request)
    {
        var registerResponse = await _persistence.ExecuteTransactionAsync(async () =>
        {
            var role = await _roleService.GetOrSave(ERole.Developer);

            var userCredential = new UserCredential
            {
                Email = request.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = role
            };
            var saveUser = await _repository.Save(userCredential);
            var developer = new Developer
            {
                Name = request.Name,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                UserCredential = saveUser
            };
            await _developerService.CreateDeveloper(developer);
            await _persistence.SaveChangesAsync();
            return new RegisterResponse
            {
                Email = saveUser.Email,
                Role = saveUser.Role.ERole.ToString()
            };
        });
        return registerResponse;
    }

    public async Task<RegisterResponse> RegisterCustomer(RegisterCustomerRequest request)
    {
        var registerResponse = await _persistence.ExecuteTransactionAsync(async () =>
        {
            var role = await _roleService.GetOrSave(ERole.Customer);

            var userCredential = new UserCredential
            {
                Email = request.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = role
            };
            var saveUser = await _repository.Save(userCredential);
            var customer = new Customer
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                City = request.City,
                PostalCode = request.PostalCode,
                Address = request.Address,
                PhoneNumber = request.PhoneNumber,
                UserCredential = userCredential
            };
            await _customerService.CreateCustomer(customer);
            await _persistence.SaveChangesAsync();
            return new RegisterResponse
            {
                Email = saveUser.Email,
                Role = saveUser.Role.ERole.ToString()
            };
        });
        return registerResponse;
    }

    public async Task<RegisterResponse> RegisterAdmin(RegisterAdminRequest request)
    {
        var registerResponse = await _persistence.ExecuteTransactionAsync(async () =>
        {
            var role = await _roleService.GetOrSave(ERole.Admin);
            var userCredential = new UserCredential
            {
                Email = request.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = role
            };
            var saveUser = await _repository.Save(userCredential);
            await _persistence.SaveChangesAsync();
            return new RegisterResponse
            {
                Email = saveUser.Email,
                Role = saveUser.Role.ERole.ToString()
            };
        });

        return registerResponse;
    }

    public async Task<LoginResponse> Login(LoginRequest request)
    {
        var user = await _repository.Find(uc => uc.Email.Equals(request.Email), new []{"Role"});
        if (user is null) throw new NotFoundException("Email not registered!");

        var verify = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);
        if (!verify) throw new UnauthorizedException("Unauthorized");

        var token = _jwtUtils.GenerateToken(user);
        var loginResponse = new LoginResponse
        {
            Email = user.Email,
            Role = user.Role.ERole.ToString(),
            Token = token
        };
        return loginResponse;
    }
}