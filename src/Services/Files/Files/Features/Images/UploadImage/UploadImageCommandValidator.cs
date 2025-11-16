using FluentValidation;

namespace Files.Features.Images.UploadImage;

public sealed class UploadImageCommandValidator : AbstractValidator<UploadImageCommand>
{
	public UploadImageCommandValidator()
	{
		RuleFor(command => command.File)
			.NotNull()
			.Must(x => x?.Length < 10 * 1024 * 1024).WithErrorCode("FileTooLarge")
			.WithMessage("The file size must be less than 10 MB.");
	}
}