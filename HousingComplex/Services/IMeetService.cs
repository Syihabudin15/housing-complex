using HousingComplex.Dto.Meet;
using HousingComplex.Entities;

namespace HousingComplex.Services;

public interface IMeetService
{
    Task<MeetResponse> CreateMeetSchedule(Meet meet);
    Task<MeetResponse> UpdateStatusMeet(string id);
}