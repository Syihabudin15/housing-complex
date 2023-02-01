using HousingComplex.DTOs;
using HousingComplex.Entities;

namespace HousingComplex.Services
{
    public interface IHouseTypeService
    {
        Task<HouseType> CreateNewHouseType(HouseType housType);
        Task<HouseType> GetForTransaction(string id);
        Task<PageResponse<HouseType>> GetAllHouseType(int page, int size);
    }
}
