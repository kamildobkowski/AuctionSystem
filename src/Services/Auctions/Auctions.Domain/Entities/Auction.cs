using System.ComponentModel.DataAnnotations;
using Auctions.Domain.Common.Enums;
using Auctions.Domain.Common.Helpers;
using Auctions.Domain.Entities.Base;

namespace Auctions.Domain.Entities;

public abstract class Auction : Aggregate<Guid>
{
	[MaxLength(200)] public string Title { get; protected set; } = null!;

	[MaxLength(500)] public string? Description { get; protected set; }

	public AuctionStatus Status { get; protected set; }

	public DateTime CreatedAt { get; protected set; }

	public DateTime SetStartDate { get; protected set; }

	public DateTime? SetEndDate { get; protected set; }

	public DateTime? EndedAt { get; protected set; }

	public Guid SellerId { get; protected set; }
	
	public AuctionStats AuctionStats { get; protected set; } = null!;

	public List<Picture> Pictures { get; protected set; } = [];

	protected Auction() { } //for serialization
	
	protected Auction(string title, string? description, DateTime? setEndDate, Guid sellerId)
	{
		Id = Guid.NewGuid();
		Title = title;
		Description = description;
		Status = AuctionStatus.Created;
		SetEndDate = setEndDate?.ToUniversalTime();
		AuctionStats = new AuctionStats(Id);
		SellerId = sellerId;
		CreatedAt = DateTimeHelper.Now;
	}
}