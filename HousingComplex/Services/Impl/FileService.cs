using System.Net.Http.Headers;

namespace HousingComplex.Services.Imp
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
    }
}
