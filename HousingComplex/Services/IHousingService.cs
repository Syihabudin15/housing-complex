using HousingComplex.DTOs;
using HousingComplex.Entities;

namespace HousingComplex.Services
{
    public interface IHousingService
    {
        Task<Housing> CreateNewHousing(Housing housing);
        Task<Housing> GetById(string id);
        Task<PageResponse<Housing>> GetAllHousing(int page, int size);
        Task<PageResponse<Housing>> SearchByCity(string city, int page, int size);
        Task<PageResponse<Housing>> SearchByName(string name, int page, int size);

    }
}
