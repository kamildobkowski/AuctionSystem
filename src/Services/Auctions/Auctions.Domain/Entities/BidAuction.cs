using Auctions.Domain.Common.Enums;
using Auctions.Domain.Common.Helpers;

namespace Auctions.Domain.Entities;

public class BidAuction : Auction
{
	private BidAuction() { } // for serialization

	public BidAuction(string title,
		string? description,
		DateTime? startDate,
		DateTime? endDate,
		decimal startingPrice,
		decimal? minimalPrice,
		Guid sellerId, 
		List<Guid> pictureIds) 
		: base(title, description, endDate, sellerId, pictureIds)
	{
		StartingPrice = startingPrice;
		CurrentPrice = startingPrice;
		MinimalPrice = minimalPrice;
		if (startDate is null)
		{
			SetStartDate = DateTimeHelper.Now;
			return;
		}
		SetStartDate = DateTimeHelper.RoundToNext10Minutes(startDate.Value.ToUniversalTime()); 
	}
	
	public decimal StartingPrice { get; private set; }

	public decimal? MinimalPrice { get; private set; }

	public decimal CurrentPrice { get; private set; }
	
	public override decimal DisplayPrice => CurrentPrice;
}