using Auctions.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auctions.Infrastructure.Database.Configuration;

public class BidAuctionConfiguration : IEntityTypeConfiguration<BidAuction>
{
	public void Configure(EntityTypeBuilder<BidAuction> builder)
	{
		builder.HasBaseType<Auction>();
	}
}