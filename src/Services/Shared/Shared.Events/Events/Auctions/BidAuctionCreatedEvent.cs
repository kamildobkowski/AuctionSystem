namespace Shared.Events.Events.Auctions;

public class BidAuctionCreatedEvent
{
	public static string Topic { get; } = "auctions.bidauction.created.v1";

	public Guid AuctionId { get; set; }

	public string Title { get; set; } = null!;

	public Guid SellerId { get; set; }

	public decimal CurrentPrice { get; set; }
}