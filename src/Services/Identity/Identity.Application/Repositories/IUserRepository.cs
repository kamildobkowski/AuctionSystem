using System.Linq.Expressions;
using Identity.Domain.Entities;

namespace Identity.Application.Repositories;

public interface IUserRepository
{
	void Add(User user);
	Task<User?> Get(Expression<Func<User, bool>> predicate);
	Task<bool> Exists(Expression<Func<User, bool>> predicate);
	Task SaveChangesAsync(CancellationToken cancellationToken = default);
}