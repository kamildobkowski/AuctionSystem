using System.Linq.Expressions;
using Identity.Application.Repositories;
using Identity.Domain.Entities;
using Identity.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Repositories;

public class UserRepository(UserDbContext dbContext) 
	: IUserRepository
{
	public void Add(User user)
	{
		dbContext.Users.Add(user);
	}

	public Task<User?> Get(Expression<Func<User, bool>> predicate)
		=> dbContext.Users.FirstOrDefaultAsync(predicate);

	public Task<bool> Exists(Expression<Func<User, bool>> predicate)
		=> dbContext.Users.AnyAsync(predicate);

	public Task SaveChangesAsync(CancellationToken cancellationToken = default)
		=> dbContext.SaveChangesAsync(cancellationToken);
}