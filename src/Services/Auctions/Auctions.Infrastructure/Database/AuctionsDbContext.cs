using Auctions.Domain.Entities;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Auctions.Infrastructure.Database;

public class AuctionsDbContext(DbContextOptions<AuctionsDbContext> options) : DbContext(options)
{
	public DbSet<Auction> Auctions { get; set; }
	public DbSet<BidAuction> BidAuctions { get; set; }

	public DbSet<BuyNowAuction> BuyNowAuctions { get; set; }

	public DbSet<AuctionStats> AuctionStats { get; set; }

	public DbSet<Picture> Pictures { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuctionsDbContext).Assembly);
		modelBuilder.AddInboxStateEntity(cfg =>
		{
			cfg.ToTable("inbox_state", "mt");
		});

		modelBuilder.AddOutboxMessageEntity(cfg =>
		{
			cfg.ToTable("outbox_message", "mt");
		});

		modelBuilder.AddOutboxStateEntity(cfg =>
		{
			cfg.ToTable("outbox_state", "mt");
		});
		modelBuilder.HasPostgresExtension("unaccent");
		base.OnModelCreating(modelBuilder);
	}
}