using Auctions.Domain.Services;
using FluentValidation;
using Shared.Base.Cqrs.Commands;
using Shared.Base.Errors;
using Shared.Base.Token;

namespace Auctions.Application.BidAuction.Create;

public sealed record CreateBidAuctionCommand(
	string Title, 
	string? Description, 
	decimal StartingPrice, 
	decimal? MinimalPrice,
	DateTime? StartDate,
	DateTime EndDate) : ICommand;

internal sealed class CreateBidAuctionCommandHandler(
	IValidator<CreateBidAuctionCommand> validator,
	IBidAuctionService bidAuctionService,
	IUserContextProvider userContextProvider) 
	: ICommandHandler<CreateBidAuctionCommand, CreateBidAuctionCommandResponse>
{
	public async Task<ICommandResult<CreateBidAuctionCommandResponse>> HandleAsync(
		CreateBidAuctionCommand command, 
		CancellationToken cancellationToken = default)
	{
		var validationResult = await validator.ValidateAsync(command, cancellationToken);
		if (validationResult.Errors.Any())
			return CommandResult.Failure<CreateBidAuctionCommandResponse>(ErrorResult.ValidationError(validationResult));

		var entity = await bidAuctionService.Create(
			title: command.Title,
			description: command.Description,
			startDate: command.StartDate,
			setEndDate: command.EndDate,
			startingPrice: command.StartingPrice,
			minimalPrice: command.MinimalPrice,
			sellerId: userContextProvider.GetUserId(),
			cancellationToken);
		
		return CommandResult.Success(new CreateBidAuctionCommandResponse(entity.Id));
	}
}