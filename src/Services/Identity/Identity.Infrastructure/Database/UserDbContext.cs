using Identity.Domain.Entities;
using Identity.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Database;

public class UserDbContext(DbContextOptions<UserDbContext> options) : DbContext(options)
{
	public DbSet<User> Users { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Owned<Address>();
		modelBuilder.Owned<PhoneNumber>();
		base.OnModelCreating(modelBuilder);
	}
}