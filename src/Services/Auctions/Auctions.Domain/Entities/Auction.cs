using Auctions.Domain.Enums;

namespace Auctions.Domain.Entities;

public abstract class Auction
{
	public Guid Id { get; set; }

	public string Title { get; set; } = null!;

	public string? Description { get; set; }

	public AuctionStatus Status { get; set; }

	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

	public DateTime SetStartDate { get; set; }

	public DateTime SetEndDate { get; set; }

	public DateTime? EndedAt { get; set; }

	public Guid SellerId { get; set; }

	
	public Guid AuctionStatsId { get; set; }

	public AuctionStats AuctionStats { get; set; } = null!;

	public List<Picture> Pictures { get; set; } = [];

	protected Auction() { } //for ef core
	
	protected Auction(string title, string? description, DateTime? startDate, DateTime setEndDate, Guid sellerId)
	{
		Id = Guid.NewGuid();
		Title = title;
		Description = description;
		Status = AuctionStatus.Scheduled;
		SetStartDate = startDate?.ToUniversalTime() ?? DateTime.UtcNow;
		SetEndDate = setEndDate.ToUniversalTime();
		AuctionStats = new AuctionStats(Id);
	}
}