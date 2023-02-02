using HousingComplex.Dto.Meet;
using HousingComplex.DTOs;
using HousingComplex.Entities;

namespace HousingComplex.Services;

public interface IMeetService
{
    Task<MeetResponse> CreateMeetSchedule(Meet meet);
    Task<MeetResponse> UpdateStatusMeet(string id);
    Task<PageResponse<MeetResponse>> GetAllSchedule(int page, int size, string email);
}