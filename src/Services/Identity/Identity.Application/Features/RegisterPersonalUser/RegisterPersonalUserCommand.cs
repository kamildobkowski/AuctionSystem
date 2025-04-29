using System.Net;
using Identity.Application.Repositories;
using Identity.Application.Services;
using Identity.Domain.Entities;
using Identity.Domain.ValueObjects;
using Shared.Base.Cqrs.Commands;
using Shared.Base.Errors;

namespace Identity.Application.Features.RegisterPersonalUser;

public record RegisterPersonalUserCommand(string Email, string Password, string RepeatPassword, string FirstName, 
	string LastName, string PhonePrefix, string PhoneNumber) : ICommand;

internal class RegisterPersonalUserCommandHandler(IUserRepository repository, IHashService hashService) 
	: ICommandHandler<RegisterPersonalUserCommand, RegisterPersonalUserResponse>
{
	public async Task<ICommandResult<RegisterPersonalUserResponse>> HandleAsync(RegisterPersonalUserCommand command, 
		CancellationToken cancellationToken = default)
	{
		var isEmailTaken = await repository.Exists(x => x.Email == command.Email);
		if (isEmailTaken)
			return CommandResult.Failure<RegisterPersonalUserResponse>(new ErrorResult
				{ StatusCode = HttpStatusCode.BadRequest });
		var hash = hashService.Hash(command.Password);
		var user = User.CreatePersonal(
			email: command.Email,
			passwordHash: hash,
			firstName: command.FirstName,
			lastName: command.LastName,
			phoneNumber: new PhoneNumber
			{
				Prefix = command.PhonePrefix,
				Number = command.PhoneNumber
			});
		repository.Add(user);
		await repository.SaveChangesAsync(cancellationToken);
		return CommandResult.Success(new RegisterPersonalUserResponse());
	}
}