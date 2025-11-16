using Files.Features.Images.Services.RemoveUnusedImages;
using Hangfire;

namespace Files.Core.RecurringJobs;

public sealed class RecurringJobsPopulator(IRecurringJobManagerV2 recurringJobManager, IRemoveUnusedImagesService service) : IRecurringJobsPopulator
{
	private const string RecurringJobId = "Files.DeleteUnusedFiles";
	
	public void Populate()
	{
		recurringJobManager.AddOrUpdate(RecurringJobId, () => service.RemoveUnusedImages(), Cron.Daily);
	}
}