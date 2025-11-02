namespace Auctions.Application.Picture.Services;

public interface IFileService
{
	Task<string> AddAuctionPhoto(Guid auctionId, FileStream fileStream, string extension);
}