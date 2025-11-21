using Auctions.Domain.Common.Enums;

namespace Auctions.Application.Contracts.AuctionList.GetUserShortList;

public sealed record UserAuctionShortListItem(
	Guid Id, 
	string Name, 
	string? Description,
	AuctionStatus Status,
	DateTime? EndDate,
	string? DefaultPictureUrl,
	decimal? CurrentPrice,
	decimal? MinimalPrice,
	AuctionType AuctionType,
	string Slug);