using Auctions.Domain.Common.Enums;
using Auctions.Domain.Common.Helpers;
using Shared.Base.Errors;
using Shared.Base.Result;

namespace Auctions.Domain.Entities;

public class BidAuction : Auction
{
	private BidAuction() { } // for serialization

	public BidAuction(string title,
		string? description,
		DateTime endDate,
		decimal startingPrice,
		decimal? minimalPrice,
		Guid sellerId) 
		: base(title, description, endDate, sellerId)
	{
		StartingPrice = startingPrice;
		CurrentPrice = startingPrice;
		MinimalPrice = minimalPrice;
	}
	
	public decimal StartingPrice { get; private set; }

	public decimal? MinimalPrice { get; private set; }

	public decimal CurrentPrice { get; private set; }

	public Result FinalizeCreate(DateTime? startDate)
	{
		if (Status != AuctionStatus.Created 
		    || startDate is not null && startDate.Value.ToUniversalTime() < DateTimeHelper.Now)
			return Result.Failure(ErrorResult.DomainError());

		if (startDate is null)
		{
			SetStartDate = DateTimeHelper.Now;
			Status = AuctionStatus.Created;
			return Result.Ok;
		}
		
		SetStartDate = DateTimeHelper.RoundToNext10Minutes(startDate.Value.ToUniversalTime());
		Status = AuctionStatus.Scheduled;
		return Result.Ok;
	}
}