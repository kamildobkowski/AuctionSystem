using FluentValidation;

namespace Identity.Application.Features.RegisterPersonalUser;

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
			.Matches(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*\W).+$");

		RuleFor(x => x.RepeatPassword)
			.NotEmpty()
			.Equal(x => x.Password);

		RuleFor(x => x.FirstName)
			.NotEmpty();

		RuleFor(x => x.LastName)
			.NotEmpty();
	}
}