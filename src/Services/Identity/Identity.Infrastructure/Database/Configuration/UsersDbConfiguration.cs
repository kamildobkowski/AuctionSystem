using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Database.Configuration;

public class UsersDbConfiguration : IEntityTypeConfiguration<User>
{
	public void Configure(EntityTypeBuilder<User> builder)
	{
		builder.ToTable("Users");
		builder.HasKey(x => x.Id);
		builder.Property(x => x.Email).IsRequired().HasMaxLength(256);
		builder.Property(x => x.PasswordHash).IsRequired();
		builder.Property(x => x.CreatedAt).IsRequired();
		builder.Property(x => x.ModifiedAt).IsRequired();
		builder.HasIndex(x => x.Email).IsUnique();
		builder.HasIndex(x => x.IsCompany);
		builder.HasOne(x => x.CompanyDetails);
		builder.HasOne(x => x.PersonalDetails);
	}
}