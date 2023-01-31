using HousingComplex.Entities;
using HousingComplex.Repositories;

namespace HousingComplex.Services.Impl;

public class DeveloperService : IDeveloperService
{
    private readonly IRepository<Developer> _repository;
    private readonly IPersistence _persistence;


    public DeveloperService(IRepository<Developer> repository, IPersistence persistence)
    {
        _repository = repository;
        _persistence = persistence;
    }

    public async Task<Developer> CreateDeveloper(Developer payload)
    {
        var developer = await _repository.Save(payload);
        await _persistence.SaveChangesAsync();
        return developer;
    }
}