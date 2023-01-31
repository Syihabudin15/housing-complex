using System.Linq.Expressions;
using HousingComplex.Dto.Login;
using HousingComplex.Dto.Register;
using HousingComplex.Entities;
using HousingComplex.Exceptions;
using HousingComplex.Repositories;
using HousingComplex.Security;
using HousingComplex.Services;
using HousingComplex.Services.Impl;
using Moq;

namespace UnitTestHousingComplex.Services;

public class AuthServiceTest
{
    private readonly Mock<IRepository<UserCredential>> _mockRepository;
    private readonly Mock<IPersistence> _mockPersistence;
    private readonly Mock<IRoleService> _mockRoleService;
    private readonly Mock<IDeveloperService> _mockDeveloperService;
    private readonly Mock<ICustomerService> _mockCustomerService;
    private readonly Mock<IJwtUtils> _mockJwtUtils;
    private readonly IAuthService _authService;

    public AuthServiceTest()
    {
        _mockRepository = new Mock<IRepository<UserCredential>>();
        _mockPersistence = new Mock<IPersistence>();
        _mockRoleService = new Mock<IRoleService>();
        _mockDeveloperService = new Mock<IDeveloperService>();
        _mockCustomerService = new Mock<ICustomerService>();
        _mockJwtUtils = new Mock<IJwtUtils>();
        _authService = new AuthService(_mockRepository.Object, _mockPersistence.Object, _mockRoleService.Object,
            _mockDeveloperService.Object, _mockCustomerService.Object, _mockJwtUtils.Object);
    }
    
    [Fact]
    public async Task Should_ReturnRegisterResponse_When_RegisterDeveloper()
    {
        var registerResponse = new RegisterResponse
        {
            Email = "proland@gmail.com",
            Role = "Developer"
        };
        var registerRequest = new RegisterDeveloperRequest
        {
            Email = "proland@gmail.com",
            Password = "password",
            Name = "Proland",
            PhoneNumber = "12345",
            Address = "Jl Pondok Rajeg"
        };
        _mockPersistence.Setup(persistence =>
                persistence.ExecuteTransactionAsync(It.IsAny<Func<Task<RegisterResponse>>>()))
            .Callback<Func<Task<RegisterResponse>>>(func => func())
            .ReturnsAsync(registerResponse);

        var result = await _authService.RegisterDeveloper(registerRequest);
        
        Assert.Equal(registerResponse.Email,result.Email);
        Assert.NotNull(result.Role);

    }
    
    [Fact]
    public async Task Should_ReturnRegisterResponse_When_RegisterCustomer()
    {
        var registerResponse = new RegisterResponse
        {
            Email = "fikri@gmail.com",
            Role = "Customer"
        };
        var registerRequest = new RegisterCustomerRequest
        {
            Email = "fikri@gmail.com",
            Password = "password",
            FirstName = "achmad",
            LastName = "fikri",
            City = "bogor",
            PostalCode = "16913",
            Address = "pejeleran gunung",
            PhoneNumber = "1234213"
        };
        _mockPersistence.Setup(persistence =>
                persistence.ExecuteTransactionAsync(It.IsAny<Func<Task<RegisterResponse>>>()))
            .Callback<Func<Task<RegisterResponse>>>(func => func())
            .ReturnsAsync(registerResponse);

        var result = await _authService.RegisterCustomer(registerRequest);
        
        Assert.Equal(registerResponse.Email,result.Email);
        Assert.NotNull(result.Role);

    }
    [Fact]
    public async Task Should_ReturnRegisterResponse_When_RegisterAdmin()
    {
        var registerResponse = new RegisterResponse
        {
            Email = "admin@gmail.com",
            Role = "Admin"
        };
        var registerRequest = new RegisterAdminRequest
        {
            Email = "admin@gmail.com",
            Password = "password"
        };
        _mockPersistence.Setup(persistence =>
                persistence.ExecuteTransactionAsync(It.IsAny<Func<Task<RegisterResponse>>>()))
            .Callback<Func<Task<RegisterResponse>>>(func => func())
            .ReturnsAsync(registerResponse);

        var result = await _authService.RegisterAdmin(registerRequest);
        
        Assert.Equal(registerResponse.Email,result.Email);
        Assert.NotNull(result.Role);

    }
    
    [Fact]
    public async Task Should_ReturnLoginResponse_When_Login()
    {
        UserCredential userCredential = new()
        {
            Id = Guid.NewGuid(),
            Email = "achmad@gmail.com",
            Password = BCrypt.Net.BCrypt.HashPassword("password"),
            RoleId = Guid.NewGuid(),
            Role = new Role
            {
                Id = Guid.NewGuid(),
                ERole = ERole.Admin
            }
        };

        var token = Guid.NewGuid().ToString();
        var loginRequest = new LoginRequest
        {
            Email = "achmad@gmail.com",
            Password = "password"
        };

        _mockRepository.Setup(repository => repository.Find(It.IsAny<Expression<Func<UserCredential,bool>>>(),
                It.IsAny<string[]>()))
            .ReturnsAsync(userCredential);
        var verify = BCrypt.Net.BCrypt.Verify(loginRequest.Password, userCredential.Password);
        
        _mockJwtUtils.Setup(jwt => jwt.GenerateToken(It.IsAny<UserCredential>()))
            .Returns(token);
        
        var result = await _authService.Login(loginRequest);
        Assert.Equal(loginRequest.Email,result.Email);
        Assert.NotNull(result.Token);
        Assert.NotNull(result.Role);
        
    }
    [Fact]
    public async Task Should_ThrowNotFoundException_When_LoginEmailNotFound()
    {
        await Assert.ThrowsAsync<NotFoundException>(() =>
            _authService.Login(new LoginRequest()));
    }
    [Fact]
    public async Task Should_UnauthorizedException_When_LoginPasswordIsWrong()
    {
        UserCredential userCredential = new()
        {
            Id = Guid.NewGuid(),
            Email = "achmad@gmail.com",
            Password = BCrypt.Net.BCrypt.HashPassword("password"),
            RoleId = Guid.NewGuid(),
            Role = new Role
            {
                Id = Guid.NewGuid(),
                ERole = ERole.Admin
            }
        };
        var loginRequest = new LoginRequest
        {
            Email = "achmad@gmail.com",
            Password = "passwordd"
        };
        _mockRepository.Setup(repository => repository.Find(
                It.IsAny<Expression<Func<UserCredential,bool>>>(),
                It.IsAny<string[]>()))
            .ReturnsAsync(userCredential);
        
        await Assert.ThrowsAsync<UnauthorizedException>(() =>
            _authService.Login(loginRequest));
    }
}