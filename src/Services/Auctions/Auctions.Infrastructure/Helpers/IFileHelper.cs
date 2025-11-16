namespace Auctions.Infrastructure.Helpers;

public interface IFileHelper
{
	string? GetFileUrl(Guid fileId);
}