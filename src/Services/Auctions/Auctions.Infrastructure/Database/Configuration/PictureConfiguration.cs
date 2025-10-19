using Auctions.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auctions.Infrastructure.Database.Configuration;

public class PictureConfiguration : IEntityTypeConfiguration<Picture>
{
	public void Configure(EntityTypeBuilder<Picture> builder)
	{
		builder.HasKey(x => x.Id);

		builder
			.HasOne(x => x.Auction)
			.WithMany(x => x.Pictures);
	}
}