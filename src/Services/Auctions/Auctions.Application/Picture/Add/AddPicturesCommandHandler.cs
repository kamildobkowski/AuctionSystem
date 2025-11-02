using Auctions.Application.Contracts.Picture;
using Auctions.Application.Picture.Services;
using Auctions.Domain.Repositories;
using FluentValidation;
using Shared.Base.Cqrs.Commands;
using Shared.Base.Errors;

namespace Auctions.Application.Picture.Add;

internal sealed class AddPicturesCommandHandler(
	IValidator<AddPicturesCommand> validator, 
	IFileService fileService,
	IAuctionRepository auctionRepository)
	: ICommandHandler<AddPicturesCommand, AddPicturesCommandResponse> 
{
	public async Task<ICommandResult<AddPicturesCommandResponse>> HandleAsync(AddPicturesCommand command, 
		CancellationToken cancellationToken = default)
	{
		var validationResult = await validator.ValidateAsync(command, cancellationToken);
		if (!validationResult.IsValid)
			return CommandResult.Failure<AddPicturesCommandResponse>(ErrorResult.ValidationError(validationResult));

		var auction = await auctionRepository.GetById(command.AuctionId, cancellationToken);
		if (auction is null)
			return CommandResult.Failure<AddPicturesCommandResponse>(new ErrorResult("Auction not found"));

		return CommandResult.Success(new AddPicturesCommandResponse());
	}
}