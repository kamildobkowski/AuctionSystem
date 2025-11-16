using Files.Core.Configuration;
using SkiaSharp;

namespace Files.Features.Images.Services;

public sealed class ImageConverter(ImageConfiguration cfg) : IImageConverter
{
    private readonly List<SKEncodedImageFormat> _allowedFormats = [SKEncodedImageFormat.Jpeg, SKEncodedImageFormat.Png];
    
    public async Task<(byte[] bytes, string extension)> Compress(byte[] imageData)
    {
        return await Task.Run(() =>
        {
            var maxWidth = cfg.MaxWidth;
            var maxHeight = cfg.MaxHeight;
            
            using var originalBitmap = SKBitmap.Decode(imageData);

            if (originalBitmap == null)
            {
                throw new ArgumentException("Could not decode image");
            }
            
            int newWidth, newHeight;
            var needsResize = originalBitmap.Width > maxWidth || originalBitmap.Height > maxHeight;

            if (needsResize)
            {
                var ratioX = (double)maxWidth / originalBitmap.Width;
                var ratioY = (double)maxHeight / originalBitmap.Height;
                var ratio = Math.Min(ratioX, ratioY);

                newWidth = (int)(originalBitmap.Width * ratio);
                newHeight = (int)(originalBitmap.Height * ratio);
            }
            else
            {
                newWidth = originalBitmap.Width;
                newHeight = originalBitmap.Height;
            }
            
            using var bitmapToEncode = new SKBitmap(newWidth, newHeight);

            if (needsResize)
            {
                originalBitmap.ScalePixels(bitmapToEncode, new SKSamplingOptions(SKFilterMode.Linear, SKMipmapMode.Linear));
            }
            else
            {
                originalBitmap.CopyTo(bitmapToEncode);
            }
            
            using var image = SKImage.FromBitmap(bitmapToEncode);
            using var data = image.Encode(SKEncodedImageFormat.Jpeg, cfg.Quality);
            return (data.ToArray(), ".jpeg");
        });
    }
    
    public async Task<bool> IsValidImageAsync(IFormFile file)
    {
        if (file.Length == 0)
        {
            return false;
        }

        try
        {
            return await Task.Run(() =>
            {
                using var stream = file.OpenReadStream();
                using var codec = SKCodec.Create(stream);

                if (codec == null)
                {
                    return false; 
                }

                var identifiedFormat = codec.EncodedFormat;

                return _allowedFormats.Any(allowed => identifiedFormat == allowed);
            });
        }
        catch (Exception)
        {
            return false;
        }
    }
}