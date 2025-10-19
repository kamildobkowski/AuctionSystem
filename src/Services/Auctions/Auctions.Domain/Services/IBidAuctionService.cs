using Auctions.Domain.Entities;

namespace Auctions.Domain.Services;

public interface IBidAuctionService
{
	Task<BidAuction> Create(
		string title, 
		string? description, 
		DateTime? startDate, 
		DateTime setEndDate, 
		decimal startingPrice, 
		decimal? minimalPrice,
		Guid sellerId,
		CancellationToken cancellationToken = default);
}