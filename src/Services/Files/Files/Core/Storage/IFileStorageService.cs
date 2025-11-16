namespace Files.Core.Storage;

public interface IFileStorageService
{
	Task<string> SaveFileAsync(byte[] fileData, string fileName);

	string GetImageFullUrl(string fileName);
}