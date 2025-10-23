using Auctions.Application.Contracts.BidAuction.Create;
using Auctions.Domain.Services;
using Auctions.Domain.UnitOfWork;
using FluentValidation;
using Shared.Base.Cqrs.Commands;
using Shared.Base.Errors;
using Shared.Base.Token;

namespace Auctions.Application.BidAuction.Create;

internal sealed class CreateBidAuctionCommandHandler(
	IValidator<CreateBidAuctionCommand> validator,
	IBidAuctionService bidAuctionService,
	IUserContextProvider userContextProvider,
	IUnitOfWork unitOfWork) 
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
			sellerId: userContextProvider.GetUserId());
		
		await unitOfWork.SaveChangesAsync(cancellationToken);
		return CommandResult.Success(new CreateBidAuctionCommandResponse(bidAuction.Id));
	}
}