using HousingComplex.DTOs;
using HousingComplex.Entities;

namespace HousingComplex.Services
{
    public interface IHousingService
    {
        Task<Housing> CreateNewHousing(Housing housing);
        Task<PageResponse<Housing>> GetAllHousing(int page, int size);
        Task<PageResponse<Housing>> SearchByCity(string city, int page, int size);
        Task<PageResponse<Housing>> SearchByName(string name, int page, int size);

    }
}
