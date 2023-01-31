using HousingComplex.Entities;

namespace HousingComplex.Services;

public interface IDeveloperService
{
    Task<Developer> CreateDeveloper(Developer payload);
}