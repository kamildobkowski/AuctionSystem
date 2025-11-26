using Auctions.Domain.Common.Enums;

namespace Auctions.Application.Contracts.AuctionDetails;

public sealed record AuctionDetailsModel(
	Guid Id,
	string Title,
	string? Description,
	DateTime? SetEndDate,
	DateTime? EndedAt,
	AuctionStatus Status,
	List<string?> PictureIds);