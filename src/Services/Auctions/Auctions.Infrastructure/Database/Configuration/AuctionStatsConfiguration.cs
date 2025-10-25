using Auctions.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auctions.Infrastructure.Database.Configuration;

public class AuctionStatsConfiguration : IEntityTypeConfiguration<AuctionStats>
{
	public void Configure(EntityTypeBuilder<AuctionStats> builder)
	{
		builder.HasKey(x => x.AuctionId);
		builder.HasOne(x => x.Auction)
			.WithOne(x => x.AuctionStats);
		builder.Property(x => x.AuctionId)
			.ValueGeneratedNever();   
	}
}