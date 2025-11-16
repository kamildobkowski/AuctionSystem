using Files.Core.Entities;
using Files.Core.Persistence;
using Files.Core.Storage;
using Files.Features.Images.Common;
using Files.Features.Images.Services;
using FluentValidation;
using Shared.Base.Cqrs.Commands;
using Shared.Base.Errors;
using Shared.Base.Token;
using Shared.Cache.Abstractions;

namespace Files.Features.Images.UploadImage;

public sealed class UploadImageCommandHandler(
	IValidator<UploadImageCommand> validator, 
	IImageConverter imageConverter,
	IFileStorageService fileStorageService,
	IUserContextProvider userContextProvider,
	ImagesDbContext dbContext,
	ICache cache) 
	: ICommandHandler<UploadImageCommand, UploadImageResponse>
{
	public async Task<ICommandResult<UploadImageResponse>> HandleAsync(
		UploadImageCommand command, CancellationToken cancellationToken = default)
	{
		var validationResult = await validator.ValidateAsync(command, cancellationToken);
		if (!validationResult.IsValid)
			return CommandResult.Failure<UploadImageResponse>(ErrorResult.ValidationError(validationResult));
		
		var userId = userContextProvider.GetUserId();
		
		await using var memoryStream = new MemoryStream();
		await command.File.CopyToAsync(memoryStream, cancellationToken);
		var imageBytes = memoryStream.ToArray();
		var (compressedImage, extension) = await imageConverter.Compress(imageBytes);

		var entity = new Image(extension, userId);
		var shortUrl = await fileStorageService.SaveFileAsync(compressedImage, entity.FileName);
		var imageUrl = fileStorageService.GetImageFullUrl(shortUrl);
		
		await cache.Set(string.Concat(CacheKeys.ImageUrlCacheKeyPrefix, entity.Id), entity.FileName);

		dbContext.Add(entity);
		await dbContext.SaveChangesAsync(cancellationToken);

		return CommandResult.Success(new UploadImageResponse(entity.Id, imageUrl));
	}
}