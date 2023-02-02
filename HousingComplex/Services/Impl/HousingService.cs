using HousingComplex.Dto.Housing;
using HousingComplex.DTOs;
using HousingComplex.Entities;
using HousingComplex.Exceptions;
using HousingComplex.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HousingComplex.Services.Impl
{
    public class HousingService : IHousingService
    {
        private readonly IRepository<Housing> _repository;
        private readonly IPersistence _persistence;
        public HousingService(IRepository<Housing> repository, IPersistence persistence)
        {
            _repository = repository;
            _persistence = persistence;
        }

        public async Task<Housing> CreateNewHousing(Housing housing)
        {
            try
            {
                var save = await _repository.Save(housing);
                await _persistence.SaveChangesAsync();
                return save;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Housing> GetById(string id)
        {
            try
            {
                var housing = await _repository.FindById(Guid.Parse(id));
                if (housing is null) throw new NotFoundException("Housing not found!");
                //var result = new Housing
                //{
                //    Id = housing.Id,
                //    Name = housing.Name,
                //    Address = housing.Address,
                //    City = housing.City,
                //    OpenTime = housing.OpenTime,
                //    Developer = housing.Developer
                //};
                //return result;
                return housing;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<PageResponse<HousingResponse>> GetAllHousing(int page, int size)
        {
            var housings = await _repository.FindAll(hous => true, page, size, new[] { "Developer" });
            try
            {
                var response = ConvetToListHousingResponse(housings);

                var totalPage = (int)Math.Ceiling((await _repository.Count()) / (decimal)size);
                PageResponse<HousingResponse> result = new()
                {
                    Content = response,
                    TotalPages = totalPage,
                    TotalElement = housings.Count()
                };
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<PageResponse<HousingResponse>> SearchByCity(string city, int page, int size)
        {
            var housings = await _repository.FindAll(hous => EF.Functions.Like(hous.City, $"%{city}%"), page, size, new[] { "Developer" });
            try
            {
                var response = ConvetToListHousingResponse(housings);

                var totalPage = (int)Math.Ceiling((await _repository.Count()) / (decimal)size);
                PageResponse<HousingResponse> result = new()
                {
                    Content = response,
                    TotalPages = totalPage,
                    TotalElement = housings.Count()
                };
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public async Task<PageResponse<HousingResponse>> SearchByName(string name, int page, int size)
        {
            var housings = await _repository.FindAll(hous => EF.Functions.Like(hous.Name, $"%{name}%"), page, size, new[] { "Developer" });
            try
            {
                var response = ConvetToListHousingResponse(housings);

                var totalPage = (int)Math.Ceiling((await _repository.Count()) / (decimal)size);
                PageResponse<HousingResponse> result = new()
                {
                    Content = response,
                    TotalPages = totalPage,
                    TotalElement = housings.Count()
                };
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public List<HousingResponse> ConvetToListHousingResponse(IEnumerable<Housing> housings)
        {
            var result = housings.Select(house => new HousingResponse
            {
                Id = house.Id.ToString(),
                Name = house.Name,
                Address = house.Address,
                City = house.City,
                OpenTime = house.OpenTime,
                Developer = new Developer
                {
                    Id = house.Developer.Id,
                    Name = house.Developer.Name,
                    Address = house.Developer.Address,
                    PhoneNumber = house.Developer.PhoneNumber,
                }
            }).ToList();
            return result;
        }

    }
}
