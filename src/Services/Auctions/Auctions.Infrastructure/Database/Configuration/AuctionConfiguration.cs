using Auctions.Domain.Common.Enums;
using Auctions.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NpgsqlTypes;

namespace Auctions.Infrastructure.Database.Configuration;

public sealed class AuctionConfiguration : IEntityTypeConfiguration<Auction>
{
	internal const string SearchVectorName = "SearchVector";
	public void Configure(EntityTypeBuilder<Auction> builder)
	{
		builder.UseTptMappingStrategy();
		
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
			.WithOne(x => x.Auction)
			.HasForeignKey<AuctionStats>(s => s.AuctionId)
				.OnDelete(DeleteBehavior.Cascade);
		
		builder.HasMany(x => x.Pictures)
			.WithOne(x => x.Auction);

		builder.Property<NpgsqlTsVector>(SearchVectorName);

		builder.HasIndex(SearchVectorName)
			.HasMethod("GIN");
	}
}