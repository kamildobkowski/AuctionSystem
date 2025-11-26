using System.Net;
using Auctions.Application.AuctionDetails.Services;
using Auctions.Application.Common.Helpers;
using Auctions.Application.Common.Jobs;
using Auctions.Application.Contracts.AuctionDetails;
using Auctions.Domain.Repositories;
using Shared.Base.Cqrs.Queries;
using Shared.Base.Errors;

namespace Auctions.Application.AuctionDetails.GetAuctionDetails;

internal sealed class GetAuctionDetailsQueryHandler(IAuctionRepository repository, GetAuctionDetailsResponseBuilder builder,
	ISellerDataService sellerDataService, IFireAndForgetScheduler fireAndForgetScheduler)
	: IQueryHandler<GetAuctionDetailsQuery, GetAuctionDetailsResponse>
{
	public async Task<IQueryResult<GetAuctionDetailsResponse>> HandleAsync(
		GetAuctionDetailsQuery query, CancellationToken cancellationToken = default)
	{
		var auction = await repository.GetById(query.Id, cancellationToken);
		if (auction is null)
			return QueryResult.Failure<GetAuctionDetailsResponse>(ErrorResult.NotFoundError);
		
		var expectedSlug = SlugHelper.Check(query.Id, query.Slug, auction.Id, auction.Title);
		if (expectedSlug is not null)
		{
			return QueryResult.Failure<GetAuctionDetailsResponse>(new ErrorResult()
			{
				StatusCode = HttpStatusCode.MovedPermanently,
				ErrorCode = "InvalidSlug",
				ErrorDescription = expectedSlug
			});
		}
		
		var sellerDataResult = await sellerDataService.GetSellerData(auction.SellerId);
		
		if (!sellerDataResult.IsSuccess)
		{
			return QueryResult.Failure<GetAuctionDetailsResponse>(sellerDataResult.ErrorResult!);
		}

		var response = builder.Build(auction, sellerDataResult.Value);

		fireAndForgetScheduler.Enqueue<IAuctionRepository>(x => x.IncrementViewCountAsync(auction.Id), cancellationToken);
		
		return QueryResult.Success(response);
	}
}