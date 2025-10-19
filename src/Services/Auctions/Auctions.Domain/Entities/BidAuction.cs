namespace Auctions.Domain.Entities;

public class BidAuction : Auction
{
	public BidAuction() { } // for ef core

	public BidAuction(string title,
		string? description,
		DateTime? startDate,
		DateTime endDate,
		decimal startingPrice,
		decimal? minimalPrice,
		Guid sellerId) 
		: base(title, description, startDate, endDate, sellerId)
	{
		StartingPrice = startingPrice;
		CurrentPrice = startingPrice;
		MinimalPrice = minimalPrice;
	}
	
	public decimal StartingPrice { get; set; }

	public decimal? MinimalPrice { get; set; }

	public decimal CurrentPrice { get; set; }
}