namespace Auctions.Domain.Entities;

public class Picture
{
	public Guid Id { get; set; }

	public bool IsPrimary { get; set; }
	
	public Guid AuctionId { get; set; }

	public Auction Auction { get; set; } = default!;

	private Picture() {}
	
	public Picture(Guid id, Guid auctionId, bool isPrimary)
	{
		Id = id;
		AuctionId = auctionId;
		IsPrimary = isPrimary;
	}
}