using System.Net;
using Files.Core.Persistence;
using Files.Core.Storage;
using Files.Features.Images.Common;
using Microsoft.EntityFrameworkCore;
using Shared.Base.Cqrs.Queries;
using Shared.Base.Errors;
using Shared.Cache.Abstractions;

namespace Files.Features.Images.GetImage;

public sealed class GetImageQueryHandler(ICache cache, IFileStorageService fileStorageService, ImagesDbContext dbContext) 
	: IQueryHandler<GetImageQuery, GetImageResponse>
{
	public async Task<IQueryResult<GetImageResponse>> HandleAsync(GetImageQuery query, CancellationToken cancellationToken = default)
	{
		var url = await cache.Get<string>(string.Concat(CacheKeys.ImageUrlCacheKeyPrefix, query.Id));
		if (url is null)
		{
			var entity = await dbContext.Images.AsNoTracking().FirstOrDefaultAsync(x => x.Id == query.Id, cancellationToken);
			if (entity is null)
				return QueryResult.Failure<GetImageResponse>(
					new ErrorResult("ImageNotFound", "Image not found", HttpStatusCode.NotFound));
			url = fileStorageService.GetImageFullUrl(entity.FileName);
			await cache.Set(string.Concat(CacheKeys.ImageUrlCacheKeyPrefix, entity.Id), entity.FileName);
		}
		var fullUrl = fileStorageService.GetImageFullUrl(url);
		return QueryResult.Success(new GetImageResponse(fullUrl));
	}
}