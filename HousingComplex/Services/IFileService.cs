using HousingComplex.Dto.ImageType;

namespace HousingComplex.Services
{
    public interface IFileService
    {
        Task<string> SaveFile(IFormFile file);
        Task<FileDownloadResponse> DownloadFile(string filepath, string filename);
    }
}
