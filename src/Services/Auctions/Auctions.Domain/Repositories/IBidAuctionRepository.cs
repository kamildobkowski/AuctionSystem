using Auctions.Domain.Entities;

namespace Auctions.Domain.Repositories;

public interface IBidAuctionRepository : IRepository<BidAuction, Guid>;