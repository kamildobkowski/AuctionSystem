namespace Files.Features.Images.Services;

public interface IImageConverter
{
	Task <(byte[] bytes, string extension)> Compress(byte[] imageData);


	Task<bool> IsValidImageAsync(IFormFile file);
}