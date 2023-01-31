using System.Net;
using HousingComplex.Controllers;
using HousingComplex.Dto;
using HousingComplex.Dto.Login;
using HousingComplex.Dto.Register;
using HousingComplex.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTestHousingComplex.Controllers;

public class AuthControllerTest
{
    private readonly Mock<IAuthService> _mockAuthService;
    private readonly AuthController _authController;

    public AuthControllerTest()
    {
        _mockAuthService = new Mock<IAuthService>();
        _authController = new AuthController(_mockAuthService.Object);
    }

    [Fact]
    public async Task Should_ReturnCreated_When_RegisterDeveloper()
    {
        var registerResponse = new RegisterResponse
        {
            Email = "proland@gmail.com",
            Role = "Developer"
        };

        var registerDeveloperRequest = new RegisterDeveloperRequest
        {
            Email = "proland@gmail.com",
            Password = "password",
            Name = "proland",
            PhoneNumber = "901238",
            Address = "Cibinong"
        };

        _mockAuthService.Setup(auth => auth.RegisterDeveloper(It.IsAny<RegisterDeveloperRequest>()))
            .ReturnsAsync(registerResponse);
        
        var commonResponse = new CommonResponse<RegisterResponse>
        {
            StatusCode = (int)HttpStatusCode.Created,
            Message = "Successfully Register",
            Data = registerResponse
        };

        var result = await _authController.RegisterDeveloper(registerDeveloperRequest) as CreatedResult;
        var resultResponse = result?.Value as CommonResponse<RegisterResponse>;

        Assert.Equal(commonResponse.StatusCode, result?.StatusCode);
        Assert.Equal(commonResponse.Data, resultResponse?.Data);
    }
    [Fact]
    public async Task Should_ReturnCreated_When_RegisterCustomer()
    {
        var registerResponse = new RegisterResponse
        {
            Email = "fadhil@gmail.com",
            Role = "Customer"
        };

        var registerCustomerRequest = new RegisterCustomerRequest
        {
            Email = "fadhil@gmail.com",
            Password = "password",
            FirstName = "fikri",
            LastName = "fadhilah",
            City = "bogor",
            PostalCode = "16913",
            Address = "cibinong",
            PhoneNumber = "1920830"
        };

        _mockAuthService.Setup(auth => auth.RegisterCustomer(It.IsAny<RegisterCustomerRequest>()))
            .ReturnsAsync(registerResponse);
        
        var commonResponse = new CommonResponse<RegisterResponse>
        {
            StatusCode = (int)HttpStatusCode.Created,
            Message = "Successfully Register",
            Data = registerResponse
        };

        var result = await _authController.RegisterCustomer(registerCustomerRequest) as CreatedResult;
        var resultResponse = result?.Value as CommonResponse<RegisterResponse>;

        Assert.Equal(commonResponse.StatusCode, result?.StatusCode);
        Assert.Equal(commonResponse.Data, resultResponse?.Data);
    }
    [Fact]
    public async Task Should_ReturnCreated_When_RegisterAdmin()
    {
        var registerResponse = new RegisterResponse
        {
            Email = "admin@gmail.com",
            Role = "Admin"
        };

        var registerAdminRequest = new RegisterAdminRequest
        {
            Email = "admin@gmail.com",
            Password = "password"
        };

        _mockAuthService.Setup(auth => auth.RegisterAdmin(It.IsAny<RegisterAdminRequest>()))
            .ReturnsAsync(registerResponse);
        
        var commonResponse = new CommonResponse<RegisterResponse>
        {
            StatusCode = (int)HttpStatusCode.Created,
            Message = "Successfully Register",
            Data = registerResponse
        };

        var result = await _authController.RegisterAdmin(registerAdminRequest) as CreatedResult;
        var resultResponse = result?.Value as CommonResponse<RegisterResponse>;

        Assert.Equal(commonResponse.StatusCode, result?.StatusCode);
        Assert.Equal(commonResponse.Data, resultResponse?.Data);
    }
    [Fact]
    public async Task Should_ReturnOk_When_Login()
    {
        var loginResponse = new LoginResponse
        {
            Email = "admin@gmail.com",
            Role = "Admin",
            Token = Guid.NewGuid().ToString()
        };
        var loginRequest = new LoginRequest
        {
            Email = "admin@gmail.com",
            Password = "password"
        };

        _mockAuthService.Setup(auth => auth.Login(It.IsAny<LoginRequest>()))
            .ReturnsAsync(loginResponse);
        
        var commonResponse = new CommonResponse<LoginResponse>
        {
            StatusCode = (int)HttpStatusCode.OK,
            Message = "Successfully Login",
            Data = loginResponse
        };

        var result = await _authController.Login(loginRequest) as OkObjectResult;
        
        var resultResponse = result?.Value as CommonResponse<LoginResponse>;
        Assert.Equal(commonResponse.StatusCode, result?.StatusCode);
        Assert.Equal(commonResponse.Data, resultResponse?.Data);
        Assert.Equal(commonResponse.Message,resultResponse.Message);
    }
}