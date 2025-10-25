using Auctions.Application.Contracts.BidAuction.InitializeCreate;
using Auctions.Domain.Repositories;
using FluentValidation;
using MassTransit;
using Shared.Base.Cqrs.Commands;
using Shared.Base.Errors;
using Shared.Base.Token;

namespace Auctions.Application.BidAuction.InitializeCreate;

internal sealed class InitializeCreateBidAuctionCommandHandler(
	IValidator<InitializeCreateBidAuctionCommand> validator,
	IUserContextProvider userContextProvider,
	IBidAuctionRepository repository) 
	: ICommandHandler<InitializeCreateBidAuctionCommand, InitializeCreateBidAuctionCommandResponse>
{
	public async Task<ICommandResult<InitializeCreateBidAuctionCommandResponse>> HandleAsync(
		InitializeCreateBidAuctionCommand command, 
		CancellationToken cancellationToken = default)
	{
		var validationResult = await validator.ValidateAsync(command, cancellationToken);
		if (validationResult.Errors.Count != 0)
			return CommandResult.Failure<InitializeCreateBidAuctionCommandResponse>(ErrorResult.ValidationError(validationResult));
		
		var bidAuction = new Domain.Entities.BidAuction(
			title: command.Title,
			description: command.Description,
			endDate: command.EndDate,
			startingPrice: command.StartingPrice,
			minimalPrice: command.MinimalPrice,
			sellerId: userContextProvider.GetUserId());
		
		repository.Add(bidAuction, cancellationToken);
		
		await repository.UnitOfWork.SaveChangesAsync(cancellationToken);
		
		return CommandResult.Success(new InitializeCreateBidAuctionCommandResponse(bidAuction.Id));
	}
}