using HousingComplex.Dto.ImageType;
using HousingComplex.DTOs;
using HousingComplex.Entities;
using Microsoft.AspNetCore.Http;

namespace HousingComplex.Services
{
    public interface IImageHouseTypeService
    {
        Task<UploadFileResponse> CreateNewImageHouseType(IFormFile file);
    }
}
