using Shared.Base.Cqrs.Queries;

namespace Auctions.Application.Contracts.AuctionList.GetUserShortList;

public sealed record GetUserAuctionShortListQuery(string? Filter = null, int PageSize = 10, int Page = 1) : IQuery;