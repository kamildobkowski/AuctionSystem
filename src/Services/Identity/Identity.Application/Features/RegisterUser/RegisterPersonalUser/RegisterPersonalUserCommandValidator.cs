using FluentValidation;
using Identity.Application.Common;

namespace Identity.Application.Features.RegisterUser.RegisterPersonalUser;

public class RegisterPersonalUserCommandValidator : AbstractValidator<RegisterPersonalUserCommand>
{
	public RegisterPersonalUserCommandValidator()
	{
		RuleFor(x => x.Email)
			.NotEmpty()
			.EmailAddress();

		RuleFor(x => x.Password)
			.NotEmpty()
			.MinimumLength(8)
			.Matches(Consts.PasswordFormatRegex);

		RuleFor(x => x.RepeatPassword)
			.NotEmpty()
			.Equal(x => x.Password);

		RuleFor(x => x.FirstName)
			.NotEmpty();

		RuleFor(x => x.LastName)
			.NotEmpty();
	}
}