using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Database.Configuration;

public class PersonalDetailsDbConfiguration : IEntityTypeConfiguration<PersonalDetails>
{
	public void Configure(EntityTypeBuilder<PersonalDetails> builder)
	{
		builder.HasKey(x => x.Id);
		builder.Property(x => x.FirstName).IsRequired().HasMaxLength(50);
		builder.Property(x => x.LastName).IsRequired().HasMaxLength(50);
		builder.OwnsOne(x => x.PhoneNumber);
	}
}