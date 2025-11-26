using System.Linq.Expressions;

namespace Auctions.Application.Common.Jobs;

public interface IFireAndForgetScheduler
{
	void Enqueue<T>(Expression<Func<T, Task>> job, CancellationToken cancellationToken = default);
}