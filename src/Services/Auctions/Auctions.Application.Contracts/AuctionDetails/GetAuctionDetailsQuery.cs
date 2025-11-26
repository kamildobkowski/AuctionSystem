using Shared.Base.Cqrs.Queries;

namespace Auctions.Application.Contracts.AuctionDetails;

public sealed record GetAuctionDetailsQuery(string Slug, Guid Id) : IQuery;