using Auctions.Domain.Entities;
using Auctions.Domain.Repositories;
using Shared.Events.EventBus;
using Shared.Events.Events.Auctions;

namespace Auctions.Domain.Services;

public class BidAuctionService(IBidAuctionRepository repository, IEventBus eventBus) : IBidAuctionService
{
	public async Task<BidAuction> Create(
		string title, 
		string? description, 
		DateTime? startDate, 
		DateTime setEndDate, 
		decimal startingPrice, 
		decimal? minimalPrice,
		Guid sellerId,
		CancellationToken cancellationToken = default)
	{
		var entity = new BidAuction(title, description, startDate, setEndDate, startingPrice, minimalPrice, sellerId);
		await repository.AddAsync(entity, cancellationToken);
		var @event = MapEvent(entity);
		await eventBus.PublishAsync(@event, cancellationToken);
		return entity;
	}

	private static BidAuctionCreatedEvent MapEvent(BidAuction entity)
	{
		return new BidAuctionCreatedEvent
		{
			AuctionId = entity.Id,
			Title = entity.Title,
			CurrentPrice = entity.CurrentPrice
		};
	}
}