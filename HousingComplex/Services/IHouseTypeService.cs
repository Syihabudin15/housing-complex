using HousingComplex.Dto.HouseType;
using HousingComplex.DTOs;
using HousingComplex.Entities;

namespace HousingComplex.Services
{
    public interface IHouseTypeService
    {
        Task<HouseType> CreateNewHouseType(HouseType housType);
        Task<HouseType> GetForTransaction(string id);
        Task<PageResponse<HouseTypeResponse>> GetAllHouseType(int page, int size);
        Task<PageResponse<HouseTypeResponse>> SearchByName(string name, int page, int size);
    }
}
