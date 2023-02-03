using System.Net;
using System.Security.Claims;
using HousingComplex.Controllers;
using HousingComplex.Dto;
using HousingComplex.Dto.Customer;
using HousingComplex.Dto.Housing;
using HousingComplex.Dto.Meet;
using HousingComplex.DTOs;
using HousingComplex.Entities;
using HousingComplex.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTestHousingComplex.Controllers;

public class MeetControllerTest
{
    private readonly Mock<IMeetService> _mockService;
    private readonly MeetController _meetController;
    private readonly Mock<ControllerBase> _controllerBase;
    private readonly Mock<ClaimsPrincipal> _mockClaimPrincipal;

    public MeetControllerTest()
    {
        _mockService = new Mock<IMeetService>();
        _meetController = new MeetController(_mockService.Object);
        _controllerBase = new Mock<ControllerBase>();
        _mockClaimPrincipal = new Mock<ClaimsPrincipal>();
    }

    [Fact]
    public async Task Should_ReturnCreated_When_SchedulingMeet()
    {
        var meetResponse = new MeetResponse
        {
            Id = Guid.NewGuid().ToString(),
            MeetDate = "2023-02-02",
            IsMeet = false,
            Housing = new HousingResponse(),
            Customer = new CustomerResponse()
        };

        var meet = new Meet
        {
            Id = Guid.NewGuid(),
            MeetDate = "2023-02-02",
            IsMeet = false,
            HousingId = Guid.NewGuid(),
            CustomerId = Guid.NewGuid(),
            Housing = new Housing(),
            Customer = new Customer()
        };
        CommonResponse<MeetResponse> response = new()
        {
            StatusCode = (int)HttpStatusCode.Created,
            Message = "Successfully create schedule to meet",
            Data = meetResponse
        };
        
        _mockService.Setup(serv => serv.CreateMeetSchedule(It.IsAny<Meet>()))
            .ReturnsAsync(meetResponse);

        var result = await _meetController.SchedulingMeet(meet) as CreatedResult;
        var resultRespon = result?.Value as CommonResponse<MeetResponse>;

        Assert.Equal(response.StatusCode,result.StatusCode);
        Assert.Equal(response.Data,resultRespon.Data);
    }

    [Fact]
    public async Task Should_ReturnOk_When_UpdateStatusMeet()
    {
        var meetResponse = new MeetResponse
        {
            Id = Guid.NewGuid().ToString(),
            MeetDate = "2023-02-02",
            IsMeet = false,
            Housing = new HousingResponse(),
            Customer = new CustomerResponse()
        };
        CommonResponse<MeetResponse> response = new()
        {
            StatusCode = (int)HttpStatusCode.OK,
            Message = "Successfully create schedule to meet",
            Data = meetResponse
        };
        _mockService.Setup(serv => serv.UpdateStatusMeet(It.IsAny<string>()))
            .ReturnsAsync(meetResponse);

        var result = await _meetController.UpdateStatusMeet(Guid.NewGuid().ToString()) as OkObjectResult;
        var resultRespon = result.Value as CommonResponse<MeetResponse>;
        
        Assert.Equal(response.StatusCode,result.StatusCode);
        Assert.Equal(response.Data,resultRespon.Data);
    }

    // [Fact]
    // public async Task Should_ReturnOK_When_GetAllScheduleMeet()
    // {
    //     // var userEmail = ;
    //     var claim = new Claim(ClaimTypes.Email,"achmad@gmail.com");
    //     var response = new PageResponse<MeetResponse>
    //     {
    //         Content = new List<MeetResponse>()
    //         {
    //             new MeetResponse()
    //             {
    //                 Id = Guid.NewGuid().ToString(),
    //                 MeetDate = "2023-02-02",
    //                 IsMeet = false,
    //                 Housing = new HousingResponse(),
    //                 Customer = new CustomerResponse()
    //             }
    //         },
    //         TotalPages = 1,
    //         TotalElement = 5
    //     };
    //     CommonResponse<PageResponse<MeetResponse>> responses = new()
    //     {
    //         StatusCode = (int)HttpStatusCode.OK,
    //         Message = "Successfully Get Schedule Meeting",
    //         Data = response
    //     };
    //     // _mockClaimsPrincipal.Setup(claim => claim.FindFirst(ClaimTypes.Email).Value)
    //     //     .Returns(userEmail);
    //     _controllerBase.Setup(@base => @base.User.FindFirst(It.IsAny<string>()))
    //         .Returns(claim);
    //     _mockClaimPrincipal.Setup(princ => princ.FindFirst(ClaimTypes.Email));
    //     // var userEmail = _controllerBase.User.FindFirst(ClaimTypes.Email).Value;
    //     _mockService.Setup(serv => serv.GetAllSchedule(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
    //         .ReturnsAsync(response);
    //
    //     var result = await _meetController.GetAllScheduleMeet(1, 5) as OkObjectResult;
    //     var resultResponse = result.Value as CommonResponse<PageResponse<MeetResponse>>;
    //     Assert.Equal(responses.StatusCode,result.StatusCode);
    //     Assert.Equal(responses.Data,resultResponse.Data);
    // }
}