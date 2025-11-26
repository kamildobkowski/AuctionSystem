using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Auctions.Domain.Common.Enums;
using Auctions.Domain.Common.Helpers;
using Auctions.Domain.Entities.Base;

namespace Auctions.Domain.Entities;

public abstract class Auction : Aggregate<Guid>
{
	[MaxLength(200)] public string Title { get; protected set; } = null!;

	[MaxLength(500)] public string? Description { get; protected set; }
	
	public AuctionStatus Status => GetStatus();

	public bool IsCancelled { get; protected set; }

	public bool IsFinished { get; protected set; }

	public DateTime CreatedAt { get; protected set; }

	public DateTime SetStartDate { get; protected set; }

	public DateTime SetEndDate { get; protected set; }

	public DateTime? EndedAt { get; protected set; }

	public Guid SellerId { get; protected set; }
	
	public AuctionStats AuctionStats { get; protected set; } = null!;

	public List<Picture> Pictures { get; protected set; } = [];

	protected Auction() { } //for serialization
	
	protected Auction(string title, string? description, DateTime? setEndDate, Guid sellerId, List<Guid> pictureIds)
	{
		Id = Guid.NewGuid();
		Title = title;
		Description = description;
		SetEndDate = setEndDate?.ToUniversalTime() ?? DateTimeHelper.Now.AddDays(7);
		AuctionStats = new AuctionStats(Id);
		SellerId = sellerId;
		CreatedAt = DateTimeHelper.Now;
		Pictures = pictureIds.Select((pid, x) => new Picture(pid, Id, x == 0)).ToList();
	}

	public abstract decimal DisplayPrice { get; }

	public virtual AuctionStatus GetStatus()
	{
		if (IsCancelled) return AuctionStatus.Cancelled;
		if (IsFinished || SetEndDate < DateTimeHelper.Now) return AuctionStatus.Ended;
		if (IsActive) return AuctionStatus.Active;
		if (IsScheduled) return AuctionStatus.Scheduled;
		return AuctionStatus.Error;
	}

	public bool IsActive => IsActiveExpression.Compile()(this);

	public bool IsScheduled => IsScheduledExpression.Compile()(this);
	
	public static Expression<Func<Auction, bool>> IsActiveExpression =>
		x => !x.IsFinished && !x.IsCancelled && x.SetEndDate > DateTimeHelper.Now && x.SetStartDate < DateTimeHelper.Now;
	
	public static Expression<Func<Auction, bool>> IsScheduledExpression =>
		x => !x.IsFinished && !x.IsCancelled && x.SetStartDate > DateTimeHelper.Now && x.SetEndDate < DateTimeHelper.Now;
}