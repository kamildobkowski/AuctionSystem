namespace Auctions.Domain.Entities;

public class AuctionStats
{
	public Guid Id { get; set; }

	public int Views { get; set; }

	public int BidderCount { get; set; }
	
	
	public Guid AuctionId { get; set; }

	public Auction Auction { get; set; } = default!;

	public AuctionStats(Guid auctionId)
	{
		Id = Guid.NewGuid();
		AuctionId = auctionId;
	}
}