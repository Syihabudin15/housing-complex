using System.Net.Http.Headers;
using HousingComplex.Dto.ImageType;
using HousingComplex.Exceptions;
using Microsoft.AspNetCore.StaticFiles;

namespace HousingComplex.Services.Impl
{
    public class FileService : IFileService
    {
        public async Task<string> SaveFile(IFormFile file)
        {
            var folderName = Path.Combine("Resource", "Image");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

            if (!Directory.Exists(pathToSave)) Directory.CreateDirectory(pathToSave);

            if (file.Length <= 0) throw new Exception();

            var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName?.Trim('"');

            var fullPath = Path.Combine(pathToSave, fileName);
            var dbPath = Path.Combine(folderName, fileName);

            await using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);

            return dbPath;
        }

        public async Task<FileDownloadResponse> DownloadFile(string filepath, string filename)
        {
            if (!File.Exists(filepath)) throw new NotFoundException("file not found");

            var memory = new MemoryStream();
            await using var stream = new FileStream(filepath, FileMode.Open);
            await stream.CopyToAsync(memory);

            memory.Position = 0;
            return new FileDownloadResponse
            {
                MemoryStream = memory,
                ContentType = GetContentType(filepath),
                Filename = filename
            };
        }
        
        private string GetContentType(string path)
        {
            var provider = new FileExtensionContentTypeProvider();

            if (!provider.TryGetContentType(path, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            return contentType;
        }
    }
}
