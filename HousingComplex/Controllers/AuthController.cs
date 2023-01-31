using System.Net;
using HousingComplex.Dto;
using HousingComplex.Dto.Login;
using HousingComplex.Dto.Register;
using HousingComplex.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HousingComplex.Controllers;

[Route("api/auth")]
public class AuthController : BaseController
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    
    [AllowAnonymous]
    [HttpPost("register-developer")]
    public async Task<IActionResult> RegisterDeveloper([FromBody] RegisterDeveloperRequest request)
    {
        var user = await _authService.RegisterDeveloper(request);
        var commonResponse = new CommonResponse<RegisterResponse>
        {
            StatusCode = (int)HttpStatusCode.Created,
            Message = "Successfully Register",
            Data = user
        };
        
        return Created("api/auth/register", commonResponse);
    }
    [AllowAnonymous]
    [HttpPost("register-customer")]
    public async Task<IActionResult> RegisterDeveloper([FromBody] RegisterCustomerRequest request)
    {
        var user = await _authService.RegisterCustomer(request);
        var commonResponse = new CommonResponse<RegisterResponse>
        {
            StatusCode = (int)HttpStatusCode.Created,
            Message = "Successfully Register",
            Data = user
        };
        
        return Created("api/auth/register", commonResponse);
    }
    
    [AllowAnonymous]
    [HttpPost("register-admin")]
    public async Task<IActionResult> RegisterAdmin([FromBody] RegisterAdminRequest request)
    {
        var user = await _authService.RegisterAdmin(request);
        var commonResponse = new CommonResponse<RegisterResponse>
        {
            StatusCode = (int)HttpStatusCode.Created,
            Message = "Successfully Register",
            Data = user
        };
        return Created("api/auth/register-admin", commonResponse);
    }
    
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _authService.Login(request);
        var commonResponse = new CommonResponse<LoginResponse>
        {
            StatusCode = (int)HttpStatusCode.OK,
            Message = "Succesfully Login",
            Data = user
        };
        return Ok(commonResponse);
    }
}