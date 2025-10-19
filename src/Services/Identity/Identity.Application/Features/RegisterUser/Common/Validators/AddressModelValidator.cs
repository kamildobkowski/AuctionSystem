using FluentValidation;
using Identity.Application.Features.RegisterUser.Common.Models;
using Maddalena;

namespace Identity.Application.Features.RegisterUser.Common.Validators;

public sealed class AddressModelValidator : AbstractValidator<AddressModel>
{
	public AddressModelValidator()
	{
		RuleFor(x => x.PostalCode)
			.NotEmpty()
			.MaximumLength(10);

		RuleFor(x => x.City)
			.NotEmpty()
			.MaximumLength(100);

		RuleFor(x => x.CountryCode)
			.NotEmpty()
			.Must(x => Enum.TryParse<CountryCode>(x, out _));
		
		RuleFor(x => x.Line1)
			.NotEmpty()
			.MaximumLength(200);

		RuleFor(x => x.Line2)
			.MaximumLength(200);
	}
}