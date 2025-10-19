namespace Auctions.Domain.Entities;

public class Picture
{
	public Guid Id { get; set; }

	public string Url { get; set; } = default!;

	public bool IsPrimary { get; set; }

	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
	
	
	public Guid? AuctionId { get; set; }

	public Auction? Auction { get; set; }
}