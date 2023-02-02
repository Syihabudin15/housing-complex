using HousingComplex.Dto.HouseType;
using HousingComplex.DTOs;
using HousingComplex.Entities;
using HousingComplex.Exceptions;
using HousingComplex.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HousingComplex.Services.Impl
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
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<HouseType> GetForTransaction(string id)
        {
            try
            {
                var houseType = await _repository.Find(type => type.Id.Equals(Guid.Parse(id)),
                    new[] { "Housing" });
                if (houseType is null) throw new NotFoundException("House Type Not Found!");
                return houseType;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<PageResponse<HouseTypeResponse>> GetAllHouseType(int page, int size)
        {
            var housTypes = await _repository.FindAll(house => true, page, size, new[] {"Spesification", "Housing.Developer", "ImageHouseType"});
            try
            {
                var result = ConvertToListHouseTypeResponse(housTypes);

                var totalPage = (int)Math.Ceiling(await _repository.Count() / (decimal)size);
                PageResponse<HouseTypeResponse> response = new()
                {
                    Content = result,
                    TotalPages = totalPage,
                    TotalElement = housTypes.Count()
                };
                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<PageResponse<HouseTypeResponse>> SearchByName(string name, int page, int size)
        {
            var houseTypes = await _repository.FindAll(house => EF.Functions.Like(house.Name, $"%{name}%"), page, size, new[] { "Spesification", "Housing.Developer", "ImageHouseType" });
            try
            {
                var result = ConvertToListHouseTypeResponse(houseTypes);
                var totalPage = (int)Math.Ceiling(await _repository.Count() / (decimal)size);
                PageResponse<HouseTypeResponse> response = new()
                {
                    Content = result,
                    TotalPages = totalPage,
                    TotalElement = houseTypes.Count()
                };
                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public List<HouseTypeResponse> ConvertToListHouseTypeResponse(IEnumerable<HouseType> houseTypes)
        {
            var result = houseTypes.Select(type => new HouseTypeResponse
            {
                Id = type.Id.ToString(),
                Name = type.Name,
                Description = type.Description,
                Price = type.Price,
                StockUnit = type.StockUnit,
                Spesification = new Spesification
                {
                    Id = type.Spesification.Id,
                    Bedrooms = type.Spesification.Bedrooms,
                    Bathrooms = type.Spesification.Bathrooms,
                    Kitchens = type.Spesification.Bathrooms,
                    Carport = type.Spesification.Carport,
                    SwimmingPool = type.Spesification.SwimmingPool,
                    SecondFloor = type.Spesification.SecondFloor
                }
            }).ToList();
            return result;
        }

    }
}
