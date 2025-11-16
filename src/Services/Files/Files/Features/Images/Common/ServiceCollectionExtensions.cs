using Files.Features.Images.GetImage;
using Files.Features.Images.Services;
using Files.Features.Images.Services.RemoveUnusedImages;
using Files.Features.Images.SetImageToUsed;
using Files.Features.Images.UploadImage;
using FluentValidation;
using Shared.Base.Cqrs.Commands;
using Shared.Base.Cqrs.Queries;
using Shared.Base.Result;
using Shared.Events.Events.Files;

namespace Files.Features.Images.Common;

public static class ServiceCollectionExtensions
{
	public static void AddImages(this IServiceCollection services)
	{
		services.AddScoped<IValidator<UploadImageCommand>, UploadImageCommandValidator>();
		services.AddScoped<ICommandHandler<UploadImageCommand, UploadImageResponse>, UploadImageCommandHandler>();
		services.AddScoped<IQueryHandler<GetImageQuery, GetImageResponse>, GetImageQueryHandler>();
		services.AddScoped<IImageConverter, ImageConverter>();
		services.AddScoped<IRemoveUnusedImagesService, RemoveUnusedImagesService>();
		services.AddScoped<ICommandHandler<SetImageToUsedCommand, NullResult>, SetImageToUsedCommandHandler>();
	}
}