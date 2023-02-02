using HousingComplex.DTOs;
using HousingComplex.Entities;
using HousingComplex.Repositories;

namespace HousingComplex.Services.Impl
{
    public class SpesificationService : ISpesificationService
    {
        private readonly IRepository<Spesification> _repository;
        private readonly IPersistence _persistence;

        public SpesificationService(IPersistence persistence, IRepository<Spesification> repository)
        {
            _persistence = persistence;
            _repository = repository;
        }

        public async Task<Spesification> CreateNewSpesification(Spesification spesification)
        {
            try
            {
                var save = await _repository.Save(spesification);
                await _persistence.SaveChangesAsync();
                return save;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PageResponse<Spesification>> GetAllSpesification(int page, int size)
        {
            var spesifications = await _repository.FindAll(spec => true, page, size);
            try
            {
                var result = spesifications.Select(spec => new Spesification
                {
                    Id = spec.Id,
                    Bedrooms = spec.Bedrooms,
                    Bathrooms = spec.Bathrooms,
                    Kitchens = spec.Kitchens,
                    Carport = spec.Carport,
                    SwimmingPool = spec.SwimmingPool,
                    SecondFloor = spec.SecondFloor
                }).ToList();

                var totalPage = (int)Math.Ceiling(await _repository.Count() / (decimal)size);
                PageResponse<Spesification> response = new()
                {
                    Content = result,
                    TotalPages = totalPage,
                    TotalElement = spesifications.Count()
                };
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
