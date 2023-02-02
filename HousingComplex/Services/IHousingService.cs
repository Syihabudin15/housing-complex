using HousingComplex.Dto.Housing;
using HousingComplex.DTOs;
using HousingComplex.Entities;

namespace HousingComplex.Services
{
    public interface IHousingService
    {
        Task<Housing> CreateNewHousing(Housing housing);
        Task<Housing> GetById(string id);
        Task<PageResponse<HousingResponse>> GetAllHousing(int page, int size);
        Task<PageResponse<HousingResponse>> SearchByCity(string city, int page, int size);
        Task<PageResponse<HousingResponse>> SearchByName(string name, int page, int size);

    }
}
