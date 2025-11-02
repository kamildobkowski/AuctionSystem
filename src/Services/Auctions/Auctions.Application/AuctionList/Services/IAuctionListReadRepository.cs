using Auctions.Application.Contracts.AuctionList.GetUserShortList;

namespace Auctions.Application.AuctionList.Services;

public interface IAuctionListReadRepository
{
	public Task<GetUserAuctionShortListQueryResponse> GetUserAuctionShortListAsync(
		Guid userId, string? filter, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
}