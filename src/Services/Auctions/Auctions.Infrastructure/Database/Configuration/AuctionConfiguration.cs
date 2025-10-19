using Auctions.Domain.Entities;
using Auctions.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auctions.Infrastructure.Database.Configuration;

public class AuctionConfiguration : IEntityTypeConfiguration<Auction>
{
	public void Configure(EntityTypeBuilder<Auction> builder)
	{
		builder.HasKey(auction => auction.Id);
		
		builder.Property(x => x.Title)
			.HasMaxLength(200);
		
		builder.Property(x => x.Description)
			.HasMaxLength(1000);
		
		builder.Property(x => x.Status)
			.HasConversion(
				x => x.ToString(), 
				x => (AuctionStatus)Enum.Parse(typeof(AuctionStatus), x));

		builder.HasOne(x => x.AuctionStats)
			.WithOne(x => x.Auction);
		
		builder.HasMany(x => x.Pictures)
			.WithOne(x => x.Auction);
	}
}