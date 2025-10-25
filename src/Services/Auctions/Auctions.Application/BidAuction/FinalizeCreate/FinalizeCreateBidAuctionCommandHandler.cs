using Auctions.Application.Contracts.BidAuction.FinalizeCreate;
using Auctions.Domain.Repositories;
using FluentValidation;
using MassTransit;
using Shared.Base.Cqrs.Commands;
using Shared.Base.Errors;
using Shared.Base.Token;
using Shared.Events.Events.Auctions;

namespace Auctions.Application.BidAuction.FinalizeCreate;

public sealed class FinalizeCreateBidAuctionCommandHandler(
	IBidAuctionRepository repository,
	IUserContextProvider userContextProvider,
	ITopicProducer<BidAuctionCreatedEvent> producer,
	IValidator<FinalizeCreateBidAuctionCommand> validator)
	: ICommandHandler<FinalizeCreateBidAuctionCommand, FinalizeCreateBidAuctionCommandResponse>
{
	public async Task<ICommandResult<FinalizeCreateBidAuctionCommandResponse>> HandleAsync(
		FinalizeCreateBidAuctionCommand command, CancellationToken cancellationToken = default)
	{
		var validationResult = await validator.ValidateAsync(command, cancellationToken);
		if (!validationResult.IsValid)
			return CommandResult.Failure<FinalizeCreateBidAuctionCommandResponse>(
				ErrorResult.ValidationError(validationResult));
		
		var userId = userContextProvider.GetUserId();
		var auction = await repository.GetFirst(x 
			=> x.Id == command.Id && x.SellerId == userId, cancellationToken);
		if (auction is null)
			return CommandResult.Failure<FinalizeCreateBidAuctionCommandResponse>(
				new ErrorResult("NotFound"));

		var result = auction.FinalizeCreate(command.StartDate);
		
		if (!result.IsSuccess)
			return CommandResult.Failure<FinalizeCreateBidAuctionCommandResponse>(result.ErrorResult!);
		
		repository.Update(auction, cancellationToken);
		
		var @event = new BidAuctionCreatedEvent(auction.Id, auction.Title, auction.SellerId, auction.CurrentPrice);
		await producer.Produce(@event, cancellationToken);
		
		await repository.UnitOfWork.SaveChangesAsync(cancellationToken);

		return CommandResult.Success(new FinalizeCreateBidAuctionCommandResponse());
	}
}