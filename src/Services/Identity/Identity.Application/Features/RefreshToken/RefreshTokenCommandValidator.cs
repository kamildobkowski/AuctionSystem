using FluentValidation;

namespace Identity.Application.Features.RefreshToken;

public sealed class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
	public RefreshTokenCommandValidator()
	{
		RuleFor(x => x.Token)
			.NotEmpty();
	}
}