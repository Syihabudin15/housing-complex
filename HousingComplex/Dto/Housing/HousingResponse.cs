using HousingComplex.Dto.Meet;
using HousingComplex.Entities;

namespace HousingComplex.Dto.Housing;

public class HousingResponse
{
    public string Id { get; set; } = String.Empty;
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string OpenTime { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public Developer? Developer { get; set; }
}