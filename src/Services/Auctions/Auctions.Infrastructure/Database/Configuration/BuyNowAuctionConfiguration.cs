using Auctions.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auctions.Infrastructure.Database.Configuration;

public class BuyNowAuctionConfiguration : IEntityTypeConfiguration<BuyNowAuction>
{
	public void Configure(EntityTypeBuilder<BuyNowAuction> builder)
	{
		builder
			.HasKey(x => x.Id);
		
		builder
			.Property(x => x.Price)
			.HasPrecision(2);
	}
}