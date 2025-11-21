using Auctions.Application.AuctionList.Services;
using Auctions.Application.Common.Helpers;
using Auctions.Application.Contracts.AuctionList.GetUserShortList;
using Auctions.Domain.Common.Enums;
using Auctions.Domain.Entities;
using Auctions.Domain.Repositories;
using Auctions.Infrastructure.Database;
using Auctions.Infrastructure.Database.Configuration;
using Auctions.Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;
using NpgsqlTypes;

namespace Auctions.Infrastructure.Repositories;

public class AuctionRepository(AuctionsDbContext dbContext, IFileHelper fileHelper) 
	: GenericRepository<Auction, Guid>(dbContext), IAuctionRepository, IAuctionListReadRepository
{
	protected override IQueryable<Auction> GetQueryable()
		=> dbContext.Auctions
			.Include(x => x.Pictures)
			.Include(x => x.AuctionStats)
			.AsNoTracking();

   public async Task<GetUserAuctionShortListQueryResponse> GetUserAuctionShortListAsync(Guid userId,
       string? filter, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
   {
       if (pageNumber < 1) pageNumber = 1;
       if (pageSize < 1) pageSize = 10;

       var query = dbContext.Auctions
          .Where(x => x.SellerId == userId);

       if (!string.IsNullOrWhiteSpace(filter))
       {
          var ftsQueryString = FtsHelper.FormatFtsQuery(filter);
          
          query = query.Where(x =>
             EF.Property<NpgsqlTsVector>(x, AuctionConfiguration.SearchVectorName).Matches(
                EF.Functions.ToTsQuery("polish", EF.Functions.Unaccent(ftsQueryString))
             ) ||
             EF.Property<NpgsqlTsVector>(x, AuctionConfiguration.SearchVectorName).Matches(
                EF.Functions.ToTsQuery("english", EF.Functions.Unaccent(ftsQueryString))
             )
          );
       }
       var totalCount = await query.CountAsync(cancellationToken);
       
       var items = await query.Skip(pageSize * (pageNumber - 1))
          .Take(pageSize)
          .AsNoTracking()
          .Select(x => new UserAuctionShortListItem(
             x.Id,
             x.Title,
             x.Description,
             x.Status,
             x.EndedAt ?? x.SetEndDate,
             x.Pictures.OrderByDescending(p => p.IsPrimary).Select(p => fileHelper.GetFileUrl(p.Id)).FirstOrDefault(),
             x is BidAuction ? (x as BidAuction)!.CurrentPrice :
                         (x is BuyNowAuction ? (x as BuyNowAuction)!.Price :
                         0),
             x is BidAuction ? (x as BidAuction)!.MinimalPrice :
                (decimal?)null, 
             x is BidAuction ? AuctionType.BidAuction : AuctionType.BuyNowAuction,
             SlugHelper.Generate(x.Id, x.Title)))
          .ToListAsync(cancellationToken);

       var hasNext = await query
          .Skip(pageSize * pageNumber)
          .AnyAsync(cancellationToken);

       var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
       
       return new GetUserAuctionShortListQueryResponse(items, totalCount, hasNext, totalPages);
   }
}