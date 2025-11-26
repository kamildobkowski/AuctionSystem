using System.Linq.Expressions;
using Auctions.Application.Common.Jobs;
using Hangfire;

namespace Auctions.Infrastructure.Schedulers;

public sealed class HangfireScheduler(IBackgroundJobClient backgroundJobClient) : IFireAndForgetScheduler
{
	public void Enqueue<T>(Expression<Func<T, Task>> job, CancellationToken cancellationToken = default)
	{
		backgroundJobClient.Enqueue(job);
	}
}