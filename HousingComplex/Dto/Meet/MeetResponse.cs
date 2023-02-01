using HousingComplex.Dto.Customer;
using HousingComplex.Dto.Housing;
using HousingComplex.Entities;

namespace HousingComplex.Dto.Meet;

public class MeetResponse
{
    public string Id { get; set; } = String.Empty;
    public string MeetDate { get; set; } = string.Empty;
    public bool IsMeet { get; set; }
    public HousingResponse? Housing { get; set; }
    public CustomerResponse? Customer { get; set; }
}