using Auctions.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Auctions.Infrastructure.Database;

public class AuctionsDbContext(DbContextOptions<AuctionsDbContext> options) : DbContext(options)
{
	public DbSet<Auction> Auctions { get; set; }

	public DbSet<BidAuction> BidAuctions { get; set; }

	public DbSet<BuyNowAuction> BuyNowAuctions { get; set; }

	public DbSet<AuctionStats> AuctionStats { get; set; }

	public DbSet<Picture> Pictures { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseNpgsql();
		base.OnConfiguring(optionsBuilder);
	}
}