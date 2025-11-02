using Auctions.Application.Picture.Services;
using Auctions.Infrastructure.Configuration;
using Minio;
using Minio.DataModel.Args;

namespace Auctions.Infrastructure.FileStorage;

public sealed class MinioFileService(MinioConfiguration config, IMinioClient minioClient) 
	: IFileService
{
	public async Task<string> AddAuctionPhoto(Guid auctionId, FileStream fileStream, string extension)
	{
		var fileName = $"{Guid.NewGuid()}.{extension}";
		var s = await minioClient.PutObjectAsync(
			new PutObjectArgs()
				.WithBucket(MinioBuckets.AuctionPhotos)
				.WithFileName(fileName)
				.WithStreamData(fileStream)
				.WithContentType($@"image\{extension}"));
		var fileUrl = $"{MinioBuckets.AuctionPhotos}/{fileName}";

		return fileUrl;
	}
}