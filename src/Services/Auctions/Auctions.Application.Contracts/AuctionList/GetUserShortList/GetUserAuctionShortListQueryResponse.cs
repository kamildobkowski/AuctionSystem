namespace Auctions.Application.Contracts.AuctionList.GetUserShortList;

public sealed record GetUserAuctionShortListQueryResponse(
	List<UserAuctionShortListItem> Auctions, int Count, bool HasNextPage, int PageCount);