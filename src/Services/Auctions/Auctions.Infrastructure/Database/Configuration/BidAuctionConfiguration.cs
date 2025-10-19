using Auctions.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auctions.Infrastructure.Database.Configuration;

public class BidAuctionConfiguration : IEntityTypeConfiguration<BidAuction>
{
	public void Configure(EntityTypeBuilder<BidAuction> builder)
	{
		builder.HasBaseType<Auction>();
		
		builder.HasKey(b => b.Id);
		
		builder.Property(b => b.StartingPrice)
			.HasPrecision(2);
		
		builder.Property(b => b.MinimalPrice)
			.HasPrecision(2);
		
		builder.Property(b => b.CurrentPrice)
			.HasPrecision(2);
	}
}