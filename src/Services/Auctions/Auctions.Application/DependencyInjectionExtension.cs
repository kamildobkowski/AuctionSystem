using Auctions.Application.AuctionList.GetUserAuctionShortList;
using Auctions.Application.BidAuction.FinalizeCreate;
using Auctions.Application.BidAuction.InitializeCreate;
using Auctions.Application.Contracts.AuctionList.GetUserShortList;
using Auctions.Application.Contracts.BidAuction.FinalizeCreate;
using Auctions.Application.Contracts.BidAuction.InitializeCreate;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Shared.Base.Cqrs.Commands;
using Shared.Base.Cqrs.Queries;

namespace Auctions.Application;

public static class DependencyInjectionExtension
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		services.AddScoped<IValidator<InitializeCreateBidAuctionCommand>, InitializeCreateBidAuctionCommandValidator>();
		services.AddScoped<IValidator<FinalizeCreateBidAuctionCommand>, FinalizeCreateBidAuctionCommandValidator>();
		
		services
			.AddScoped<ICommandHandler<InitializeCreateBidAuctionCommand, 
				InitializeCreateBidAuctionCommandResponse>, InitializeCreateBidAuctionCommandHandler>();
		services
			.AddScoped<ICommandHandler<FinalizeCreateBidAuctionCommand, FinalizeCreateBidAuctionCommandResponse>, 
				FinalizeCreateBidAuctionCommandHandler>();
		services
			.AddScoped<IQueryHandler<GetUserAuctionShortListQuery, GetUserAuctionShortListQueryResponse>,
				GetUserAuctionShortListQueryHandler>();
		return services;
	}
}