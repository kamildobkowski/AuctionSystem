using Auctions.Application.AuctionList.Services;
using Auctions.Application.Common.Extensions;
using Auctions.Application.Common.Helpers;
using Auctions.Application.Contracts.AuctionList.GetUserShortList;
using Auctions.Domain.Entities;
using Auctions.Domain.Repositories;
using Auctions.Infrastructure.Database;
using Auctions.Infrastructure.Database.Configuration;
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
          .Include(x => x.Pictures)
          .ToListAsync(cancellationToken);
          
       var responseItems = items
          .Select(x => new UserAuctionShortListItem(
             x.Id,
             x.Title,
             x.Description,
             x.GetStatus(),
             x.EndedAt ?? x.SetEndDate,
             x.Pictures.OrderByDescending(p => p.IsPrimary).Select(p => fileHelper.GetFileUrl(p.Id)).FirstOrDefault(),
             x.DisplayPrice,
             x.GetAuctionType(),
             SlugHelper.Generate(x.Id, x.Title)))
          .ToList();

       var hasNext = await query
          .Skip(pageSize * pageNumber)
          .AnyAsync(cancellationToken);

       var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
       
       return new GetUserAuctionShortListQueryResponse(responseItems, totalCount, hasNext, totalPages);
   }

   public Task IncrementViewCountAsync(Guid auctionId)
      => dbContext.AuctionStats
         .Where(x => x.AuctionId == auctionId)
         .ExecuteUpdateAsync(x => x
            .SetProperty(x => x.Views, x => x.Views + 1));
}