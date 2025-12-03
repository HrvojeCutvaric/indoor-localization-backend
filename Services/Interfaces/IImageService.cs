namespace IndoorLocalization.Services.Interfaces
{
    public interface IImageService
    {
        Task<string> UploadImageAsync(IFormFile imageFile);
    }
}
