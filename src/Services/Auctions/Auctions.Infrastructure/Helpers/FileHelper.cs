using Auctions.Infrastructure.Configuration;

namespace Auctions.Infrastructure.Helpers;

public sealed class FileHelper(FileConfiguration configuration) 
	: IFileHelper
{
	public string? GetFileUrl(Guid fileId)
		=> configuration.FileUrl(fileId.ToString());
}