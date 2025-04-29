using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Database.Configuration;

public class CompanyDetailsDbConfiguration : IEntityTypeConfiguration<CompanyDetails>
{
	public void Configure(EntityTypeBuilder<CompanyDetails> builder)
	{
		builder.HasKey(x => x.Id);
		builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
		builder.Property(x => x.Nip).HasMaxLength(12);
		builder.OwnsOne(x => x.PhoneNumber);
		builder.OwnsOne(x => x.Address);
	}
}