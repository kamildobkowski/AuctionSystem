using System.ComponentModel.DataAnnotations;

namespace Auctions.Domain.Entities;

public class Picture
{
	public Guid Id { get; set; }

	[MaxLength(200)] public string Url { get; set; } = default!;

	public bool IsPrimary { get; set; }

	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
	
	public Guid? AuctionId { get; set; }

	public Auction? Auction { get; set; }
	
	public Picture(Guid auctionId, string url, bool isPrimary)
	{
		AuctionId = auctionId;
		Url = url;
		IsPrimary = isPrimary;
	}
	
	private Picture(){}
}