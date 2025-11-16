using Auctions.Application.Contracts.BidAuction.InitializeCreate;
using Auctions.Domain.Common.Helpers;
using FluentValidation;

namespace Auctions.Application.BidAuction.InitializeCreate;

public sealed class CreateBidAuctionCommandValidator : AbstractValidator<CreateBidAuctionCommand>
{
	public CreateBidAuctionCommandValidator()
	{
		RuleFor(x => x.Title)
			.NotEmpty()
			.MaximumLength(200);
		
		RuleFor(x => x.Description)
			.MaximumLength(500);

		RuleFor(x => x.MinimalPrice)
			.GreaterThan(0);
		
		RuleFor(x => x.StartingPrice)
			.NotNull()
			.GreaterThan(0);
		
		RuleFor(x => x.StartDate)
			.GreaterThanOrEqualTo(DateTimeHelper.Next10Minutes);
	}
}