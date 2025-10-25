using Auctions.Application.Contracts.BidAuction.FinalizeCreate;
using Auctions.Domain.Common.Helpers;
using FluentValidation;

namespace Auctions.Application.BidAuction.FinalizeCreate;

public sealed class FinalizeCreateBidAuctionCommandValidator : AbstractValidator<FinalizeCreateBidAuctionCommand>
{
	public FinalizeCreateBidAuctionCommandValidator()
	{
		RuleFor(x => x.Id).NotEmpty();

		RuleFor(x => x.StartDate)
			.GreaterThanOrEqualTo(DateTimeHelper.Next10Minutes);
	}
}