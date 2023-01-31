using HousingComplex.DTOs;
using HousingComplex.Entities;
using HousingComplex.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HousingComplex.Services.Imp
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
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PageResponse<Housing>> GetAllHousing(int page, int size)
        {
            var housings = await _repository.FindAll(hous => true, page, size, new[] { "Developer" });
            try
            {
                var response = ConvetToListHousing(housings);

                var totalPage = (int)Math.Ceiling((await _repository.Count()) / (decimal)size);
                PageResponse<Housing> result = new()
                {
                    Content = response,
                    TotalPages = totalPage,
                    TotalElement = housings.Count()
                };
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PageResponse<Housing>> SearchByCity(string city, int page, int size)
        {
            var housings = await _repository.FindAll(hous => EF.Functions.Like(hous.City, $"%{city}%"), page, size, new[] { "Developer" });
            try
            {
                var response = ConvetToListHousing(housings);

                var totalPage = (int)Math.Ceiling((await _repository.Count()) / (decimal)size);
                PageResponse<Housing> result = new()
                {
                    Content = response,
                    TotalPages = totalPage,
                    TotalElement = housings.Count()
                };
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<PageResponse<Housing>> SearchByName(string name, int page, int size)
        {
            var housings = await _repository.FindAll(hous => EF.Functions.Like(hous.Name, $"%{name}%"), page, size, new[] { "Developer" });
            try
            {
                var response = ConvetToListHousing(housings);

                var totalPage = (int)Math.Ceiling((await _repository.Count()) / (decimal)size);
                PageResponse<Housing> result = new()
                {
                    Content = response,
                    TotalPages = totalPage,
                    TotalElement = housings.Count()
                };
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Housing> ConvetToListHousing(IEnumerable<Housing> housings)
        {
            var response = housings.Select(hous => new Housing
            {
                Id = hous.Id,
                Name = hous.Name,
                Address = hous.Address,
                City = hous.City,
                OpenTime = hous.OpenTime,
                DeveloperId = hous.DeveloperId,
                Developer = hous.Developer
            }).ToList();
            return response;
        }

    }
}
