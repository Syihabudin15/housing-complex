using HousingComplex.DTOs;
using HousingComplex.Entities;
using HousingComplex.Repositories;

namespace HousingComplex.Services.Imp
{
    public class HouseTypeService : IHouseTypeService
    {
        private readonly IRepository<HouseType> _repository;
        private readonly IPersistence _persistence;

        public HouseTypeService(IPersistence persistence, IRepository<HouseType> repository)
        {
            _persistence = persistence;
            _repository = repository;
        }

        public async Task<HouseType> CreateNewHouseType(HouseType housType)
        {
            try
            {
                var response = await _repository.Save(housType);
                await _persistence.SaveChangesAsync();
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PageResponse<HouseType>> GetAllHouseType(int page, int size)
        {
            var housTypes = await _repository.FindAll(house => true, page, size, new[] {"Spesification", "Housing.Developer", "ImageHouseType"});
            try
            {
                var result = ConvertToList(housTypes);

                var totalPage = (int)Math.Ceiling(await _repository.Count() / (decimal)size);
                PageResponse<HouseType> response = new()
                {
                    Content = result,
                    TotalPages = totalPage,
                    TotalElement = housTypes.Count()
                };
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<HouseType> ConvertToList(IEnumerable<HouseType> houseTypes)
        {
            var listHouseType = houseTypes.Select(house => new HouseType
            {
                Id = house.Id,
                Name = house.Name,
                Description = house.Description,
                Price = house.Price,
                StockUnit = house.StockUnit,
                SpesificationId = house.SpesificationId,
                HousingId = house.HousingId,
                ImageId = house.ImageId,
                Spesification = house.Spesification,
                Housing = house.Housing,
                ImageHouseType = house.ImageHouseType
            }).ToList();
            return listHouseType;
        }
        
    }
}
