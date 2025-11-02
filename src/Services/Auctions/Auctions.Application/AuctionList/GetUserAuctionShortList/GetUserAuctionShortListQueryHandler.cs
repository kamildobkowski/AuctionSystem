using Auctions.Application.AuctionList.Services;
using Auctions.Application.Contracts.AuctionList.GetUserShortList;
using Shared.Base.Cqrs.Queries;
using Shared.Base.Token;

namespace Auctions.Application.AuctionList.GetUserAuctionShortList;

internal sealed class GetUserAuctionShortListQueryHandler(
	IUserContextProvider userContextProvider,
	IAuctionListReadRepository auctionRepository) 
	: IQueryHandler<GetUserAuctionShortListQuery, GetUserAuctionShortListQueryResponse>
{
	public async Task<IQueryResult<GetUserAuctionShortListQueryResponse>> HandleAsync(
		GetUserAuctionShortListQuery query, CancellationToken cancellationToken = default)
	{
		var userId = userContextProvider.GetUserId();
		var response = await auctionRepository.GetUserAuctionShortListAsync(
			userId, query.Filter, query.Page, query.PageSize, cancellationToken);
		return QueryResult.Success(response);
	}
}