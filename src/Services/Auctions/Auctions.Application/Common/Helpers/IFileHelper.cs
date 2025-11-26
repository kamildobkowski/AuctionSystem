namespace Auctions.Application.Common.Helpers;

public interface IFileHelper
{
	string? GetFileUrl(Guid fileId);
}