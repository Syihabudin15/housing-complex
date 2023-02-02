using System.Net;
using System.Security.Claims;
using HousingComplex.Dto;
using HousingComplex.Dto.Meet;
using HousingComplex.DTOs;
using HousingComplex.Entities;
using HousingComplex.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HousingComplex.Controllers;

[ApiController]
[Route("api/meet")]
public class MeetController :  BaseController
{
    private readonly IMeetService _meetService;

    public MeetController(IMeetService meetService)
    {
        _meetService = meetService;
    }

    [HttpPost]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> SchedulingMeet(Meet request)
    {
        var result = await _meetService.CreateMeetSchedule(request);
        CommonResponse<MeetResponse> response = new()
        {
            StatusCode = (int)HttpStatusCode.Created,
            Message = "Successfully create schedule to meet",
            Data = result
        };
        return Created("api/meet", response);
    }
    [HttpPost("update/{id}")]
    [Authorize(Roles = "Developer")]
    public async Task<IActionResult> UpdateStatusMeet(string id)
    {
        var result = await _meetService.UpdateStatusMeet(id);
        CommonResponse<MeetResponse> response = new()
        {
            StatusCode = (int)HttpStatusCode.OK,
            Message = "Successfully Update Status Meeting",
            Data = result
        };
        return Ok(response);
    }
    [HttpGet("dev")]
    [Authorize(Roles = "Developer")]
    public async Task<IActionResult> GetAllScheduleMeet([FromQuery]int page, [FromQuery] int size)
    {
        var userEmail = User.FindFirst(ClaimTypes.Email).Value;
        var result = await _meetService.GetAllSchedule(page,size,userEmail);
        CommonResponse<PageResponse<MeetResponse>> response = new()
        {
            StatusCode = (int)HttpStatusCode.OK,
            Message = "Successfully Get Schedule Meeting",
            Data = result
        };
        return Ok(response);
    }
}