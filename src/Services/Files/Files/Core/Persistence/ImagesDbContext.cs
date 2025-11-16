using Files.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Files.Core.Persistence;

public sealed class ImagesDbContext(DbContextOptions options) : DbContext(options)
{
	public DbSet<Image> Images { get; init; }
}