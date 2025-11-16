using Files.Core.Configuration;
using Minio;
using Minio.DataModel.Args;

namespace Files.Core.Storage;

public sealed class MinioStorageService(IMinioClient minioClient, FileStorageConfiguration configuration) : IFileStorageService
{
	public async Task<string> SaveFileAsync(byte[] fileData, string fileName)
	{
		using var stream = new MemoryStream(fileData);

		var contentType = fileName.EndsWith(".png") ? "image/png" :
		                  fileName.EndsWith(".jpg") || fileName.EndsWith(".jpeg") ? "image/jpeg" :
		                  "application/octet-stream";

		var putObjectArgs = new PutObjectArgs()
			.WithBucket("images")
			.WithObject(fileName)
			.WithStreamData(stream)
			.WithObjectSize(stream.Length)
			.WithContentType(contentType);

		await minioClient.PutObjectAsync(putObjectArgs);
		
		return string.Concat($"/{Buckets.ImagesBucket}/", fileName);
	}

	public string GetImageFullUrl(string fileName)
		=> string.Concat(
			configuration.PublicUrl,
			fileName.Contains(Buckets.ImagesBucket) ? null : $"/{Buckets.ImagesBucket}/", 
			fileName);
}