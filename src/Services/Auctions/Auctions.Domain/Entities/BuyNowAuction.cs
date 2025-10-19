namespace Auctions.Domain.Entities;

public class BuyNowAuction : Auction
{
	public BuyNowAuction() {} //for ef core
	
	public BuyNowAuction(
		string title,
		string? description, 
		DateTime? startDate, 
		DateTime setEndDate, 
		decimal price,
		Guid sellerId) 
		: base(title, description, startDate, setEndDate, sellerId)
	{
		Price = price;
	}

	public decimal Price { get; set; }
}