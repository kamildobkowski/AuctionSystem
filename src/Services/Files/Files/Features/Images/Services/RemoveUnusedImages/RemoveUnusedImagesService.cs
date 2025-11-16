using Files.Core.Persistence;
using Files.Core.Storage;
using Microsoft.EntityFrameworkCore;

namespace Files.Features.Images.Services.RemoveUnusedImages;

public sealed class RemoveUnusedImagesService(ImagesDbContext dbContext, IFileStorageService fileStorageService) : IRemoveUnusedImagesService
{
	public async Task RemoveUnusedImages()
	{
		var list = await dbContext.Images
			.Where(x => x.LastModified < DateTime.UtcNow.AddDays(-1) && !x.IsUsed)
			.ToListAsync();
		
		list.ForEach(x => x.IsUsed = false);

		await dbContext.SaveChangesAsync();
	}
}