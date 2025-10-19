using System.Linq.Expressions;
using Identity.Domain.Entities;
using Identity.Domain.Repositories;
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
		=> dbContext
			.Users
			.Include(x => x.CompanyDetails)
			.Include(x => x.PersonalDetails)
			.FirstOrDefaultAsync(predicate);

	public Task<bool> Exists(Expression<Func<User, bool>> predicate)
		=> dbContext.Users.AnyAsync(predicate);

	public Task<User?> GetByRefreshToken(string hashedRefreshToken)
		=> Get(x => x.RefreshTokenHash == hashedRefreshToken);

	public Task SaveChangesAsync(CancellationToken cancellationToken = default)
		=> dbContext.SaveChangesAsync(cancellationToken);
}