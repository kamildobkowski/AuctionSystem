namespace Auctions.Domain.Entities;

public class BuyNowAuction : Auction
{
	private BuyNowAuction() {} //for serialization
	
	public BuyNowAuction(
		string title,
		string? description, 
		DateTime setEndDate, 
		decimal price,
		Guid sellerId) 
		: base(title, description, setEndDate, sellerId)
	{
		Price = price;
	}

	public decimal Price { get; private set; }
}