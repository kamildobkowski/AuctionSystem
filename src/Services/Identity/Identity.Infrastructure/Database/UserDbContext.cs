using Identity.Domain.Entities;
using Identity.Domain.ValueObjects;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Database;

public class UserDbContext(DbContextOptions<UserDbContext> options) : DbContext(options)
{
	public DbSet<User> Users { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Owned<Address>();
		modelBuilder.Owned<PhoneNumber>();
		modelBuilder.AddInboxStateEntity(cfg =>
		{
			cfg.ToTable("inbox_state", "mt");
		});

		modelBuilder.AddOutboxMessageEntity(cfg =>
		{
			cfg.ToTable("outbox_message", "mt");
		});

		modelBuilder.AddOutboxStateEntity(cfg =>
		{
			cfg.ToTable("outbox_state", "mt");
		});
		base.OnModelCreating(modelBuilder);
	}
}