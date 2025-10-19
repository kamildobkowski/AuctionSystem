using System.Linq.Expressions;
using Identity.Domain.Entities;

namespace Identity.Domain.Repositories;

public interface IUserRepository
{
	void Add(User user);
	Task<User?> Get(Expression<Func<User, bool>> predicate);
	Task<bool> Exists(Expression<Func<User, bool>> predicate);
	Task<User?> GetByRefreshToken(string hashedRefreshToken);
	Task SaveChangesAsync(CancellationToken cancellationToken = default);
}