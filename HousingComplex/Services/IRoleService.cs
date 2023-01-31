using HousingComplex.Entities;

namespace HousingComplex.Services;

public interface IRoleService
{
    Task<Role> GetOrSave(ERole role);
}