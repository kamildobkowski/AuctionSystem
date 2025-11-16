using Shared.Base.Cqrs.Commands;

namespace Files.Features.Images.UploadImage;

public sealed record UploadImageCommand(IFormFile File) : ICommand;