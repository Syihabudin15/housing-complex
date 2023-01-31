using HousingComplex.Entities;
using HousingComplex.Repositories;

namespace HousingComplex.Services.Impl;

public class RoleService : IRoleService
{
    private readonly IRepository<Role> _repository;
    private readonly IPersistence _persistence;

    public RoleService(IRepository<Role> repository, IPersistence persistence)
    {
        _repository = repository;
        _persistence = persistence;
    }

    public async Task<Role> GetOrSave(ERole role)
    {
        var roleFind = await _repository.Find(r => r.ERole.Equals(role));
        if (roleFind is not null) return roleFind;

        var saveRole = await _repository.Save(new Role { ERole = role });
        await _persistence.SaveChangesAsync();
        return saveRole;
    }
}