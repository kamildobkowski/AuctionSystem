using FluentValidation;
using Identity.Application.Common;
using Identity.Application.Features.RegisterUser.Common.Models;

namespace Identity.Application.Features.RegisterUser.RegisterCompanyUser;

public sealed class RegisterCompanyUserCommandValidator : AbstractValidator<RegisterCompanyUserCommand>
{
	public RegisterCompanyUserCommandValidator(IValidator<AddressModel> addressModelValidator)
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

		RuleFor(x => x.Name)
			.NotEmpty();

		RuleFor(x => x.TaxId)
			.NotEmpty();

		RuleFor(x => x.Address)
			.SetValidator(addressModelValidator);
	}
}