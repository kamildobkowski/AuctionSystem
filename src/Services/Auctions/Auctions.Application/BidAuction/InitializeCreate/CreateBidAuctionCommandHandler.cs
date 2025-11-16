using Auctions.Application.Common.Helpers;
using Auctions.Application.Contracts.BidAuction.InitializeCreate;
using Auctions.Domain.Repositories;
using FluentValidation;
using MassTransit;
using Shared.Base.Cqrs.Commands;
using Shared.Base.Errors;
using Shared.Base.Token;
using Shared.Events.Events.Auctions;
using Shared.Events.Events.Files;

namespace Auctions.Application.BidAuction.InitializeCreate;

internal sealed class CreateBidAuctionCommandHandler(
	IValidator<CreateBidAuctionCommand> validator,
	IUserContextProvider userContextProvider,
	IBidAuctionRepository repository,
	ITopicProducer<BidAuctionCreatedEvent> producer,
	ITopicProducer<SetImageToUsedCommand> setImageToUsedProducer) 
	: ICommandHandler<CreateBidAuctionCommand, CreateBidAuctionCommandResponse>
{
	public async Task<ICommandResult<CreateBidAuctionCommandResponse>> HandleAsync(
		CreateBidAuctionCommand command, 
		CancellationToken cancellationToken = default)
	{
		var validationResult = await validator.ValidateAsync(command, cancellationToken);
		if (validationResult.Errors.Count != 0)
			return CommandResult.Failure<CreateBidAuctionCommandResponse>(ErrorResult.ValidationError(validationResult));
		
		var bidAuction = new Domain.Entities.BidAuction(
			title: command.Title,
			description: command.Description,
			startDate: command.StartDate,
			endDate: command.EndDate,
			startingPrice: command.StartingPrice,
			minimalPrice: command.MinimalPrice,
			sellerId: userContextProvider.GetUserId(),
			pictureIds: command.PictureIds);
		
		repository.Add(bidAuction, cancellationToken);
		
		var @event = new BidAuctionCreatedEvent(bidAuction.Id, bidAuction.Title, bidAuction.SellerId, bidAuction.CurrentPrice);
		await producer.Produce(@event, cancellationToken);
		
		foreach (var pictureId in command.PictureIds)
		{
			var setImageToUsedCommand = new SetImageToUsedCommand { ImageId = pictureId };
			await setImageToUsedProducer.Produce(setImageToUsedCommand, cancellationToken);
		}
		
		await repository.UnitOfWork.SaveChangesAsync(cancellationToken);
		
		return CommandResult.Success(new CreateBidAuctionCommandResponse(bidAuction.Id, SlugHelper.Generate(bidAuction.Id, bidAuction.Title)));
	}
}