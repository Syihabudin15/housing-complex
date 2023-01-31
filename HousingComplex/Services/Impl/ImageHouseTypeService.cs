using HousingComplex.Dto.ImageType;
using HousingComplex.DTOs;
using HousingComplex.Entities;
using HousingComplex.Repositories;

namespace HousingComplex.Services.Impl
{
    public class ImageHouseTypeService : IImageHouseTypeService
    {
        private readonly IRepository<ImageHouseType> _repository;
        private readonly IFileService _fileService;
        private readonly IPersistence _persistence;

        public ImageHouseTypeService(IPersistence persistence, IRepository<ImageHouseType> repository, IFileService fileService)
        {
            _persistence = persistence;
            _repository = repository;
            _fileService = fileService;
        }

        public async Task<UploadFileResponse> CreateNewImageHouseType(IFormFile file)
        {
            var saveFile = await _fileService.SaveFile(file);
            var profilePicture = await _repository.Save(new ImageHouseType
            {
                FileName = file.FileName,
                FileSize = file.Length,
                FilePath = saveFile,
                ContentType = file.ContentType
            });
            await _persistence.SaveChangesAsync();
            return new UploadFileResponse
            {
                Id = profilePicture.Id.ToString(),
                FileName = profilePicture.FileName,
                ContentType = profilePicture.ContentType
            };
        }
    }
}
