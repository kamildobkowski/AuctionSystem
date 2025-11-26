using Auctions.Application.AuctionDetails.GetAuctionDetails;
using Auctions.Application.AuctionList.GetUserAuctionShortList;
using Auctions.Application.BidAuctions.InitializeCreate;
using Auctions.Application.Contracts.AuctionDetails;
using Auctions.Application.Contracts.AuctionList.GetUserShortList;
using Auctions.Application.Contracts.BidAuctions.InitializeCreate;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Shared.Base.Cqrs.Commands;
using Shared.Base.Cqrs.Queries;

namespace Auctions.Application;

public static class DependencyInjectionExtension
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		services.AddScoped<IValidator<CreateBidAuctionCommand>, CreateBidAuctionCommandValidator>();
		services
			.AddScoped<ICommandHandler<CreateBidAuctionCommand, 
				CreateBidAuctionCommandResponse>, CreateBidAuctionCommandHandler>();
		services
			.AddScoped<IQueryHandler<GetUserAuctionShortListQuery, GetUserAuctionShortListQueryResponse>,
				GetUserAuctionShortListQueryHandler>();
		services
			.AddScoped<IQueryHandler<GetAuctionDetailsQuery, GetAuctionDetailsResponse>,
				GetAuctionDetailsQueryHandler>();
		services.AddScoped<GetAuctionDetailsResponseBuilder>();
		return services;
	}
}