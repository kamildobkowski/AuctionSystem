using Auctions.Application.BidAuction.FinalizeCreate;
using Auctions.Application.BidAuction.InitializeCreate;
using Auctions.Application.Contracts.BidAuction.FinalizeCreate;
using Auctions.Application.Contracts.BidAuction.InitializeCreate;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Shared.Base.Cqrs.Commands;

namespace Auctions.Application;

public static class DependencyInjectionExtension
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		services.AddScoped<IValidator<InitializeCreateBidAuctionCommand>, InitializeCreateBidAuctionCommandValidator>();
		services.AddScoped<IValidator<FinalizeCreateBidAuctionCommand>, FinalizeCreateBidAuctionCommandValidator>();
		
		services.AddScoped<ICommandHandler<InitializeCreateBidAuctionCommand, InitializeCreateBidAuctionCommandResponse>, InitializeCreateBidAuctionCommandHandler>();
		services.AddScoped<ICommandHandler<FinalizeCreateBidAuctionCommand, FinalizeCreateBidAuctionCommandResponse>, FinalizeCreateBidAuctionCommandHandler>();
		
		return services;
	}
}