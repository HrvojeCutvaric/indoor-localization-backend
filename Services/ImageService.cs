using IndoorLocalization.Services.Interfaces;

namespace IndoorLocalization.Services
{
    public class ImageService : IImageService
    {
        private readonly string _imageStoragePath;

        public ImageService(IWebHostEnvironment env)
        {
            _imageStoragePath = Path.Combine(env.WebRootPath, "images");
        }

        public async Task<string> UploadImageAsync(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                throw new ArgumentException("Invalid image file.");

            var fileName = $"{Guid.NewGuid()}_{imageFile.FileName}";
            var filePath = Path.Combine(_imageStoragePath, fileName);

            Directory.CreateDirectory(_imageStoragePath);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return $"/images/{fileName}";
        }

        public async Task<bool> DeleteImageAsync(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
                return false;

            var fileName = Path.GetFileName(imageUrl);
            var filePath = Path.Combine(_imageStoragePath, fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return true;
            }

            return false;
        }
    }
}
