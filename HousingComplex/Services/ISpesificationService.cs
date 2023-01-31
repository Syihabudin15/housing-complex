using HousingComplex.DTOs;
using HousingComplex.Entities;

namespace HousingComplex.Services
{
    public interface ISpesificationService
    {
        Task<Spesification> CreateNewSpesification(Spesification spesification);
        Task<PageResponse<Spesification>> GetAllSpesification(int page, int size);
    }
}
