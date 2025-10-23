using System.Net;
using FluentValidation;
using Identity.Domain.Entities;
using Identity.Domain.Repositories;
using Identity.Domain.Services;
using Identity.Domain.ValueObjects;
using MassTransit;
using Microsoft.FeatureManagement;
using Shared.Base.Cqrs.Commands;
using Shared.Base.Errors;
using Shared.Events.Events.Users;

namespace Identity.Application.Features.RegisterUser.RegisterPersonalUser;

public record RegisterPersonalUserCommand(string Email, string Password, string RepeatPassword, string FirstName, 
	string LastName, string PhonePrefix, string PhoneNumber) : ICommand;

internal sealed class RegisterPersonalUserCommandHandler(IValidator<RegisterPersonalUserCommand> validator, IUserRepository repository, 
	IHashService hashService, ICacheService cacheService, IFeatureManager featureManager,
	IActivationCodeGenerator codeGenerator, ITopicProducer<PersonalUserCreatedEvent> producer) 
	: ICommandHandler<RegisterPersonalUserCommand, RegisterPersonalUserResponse>
{
	public async Task<ICommandResult<RegisterPersonalUserResponse>> HandleAsync(RegisterPersonalUserCommand command, 
		CancellationToken cancellationToken = default)
	{
		var result = await validator.ValidateAsync(command, cancellationToken);
		if (!result.IsValid)
		{
			return CommandResult.Failure<RegisterPersonalUserResponse>(ErrorResult.ValidationError(result));
		}
		
		var isEmailTaken = await cacheService.IsEmailTaken(command.Email);
		if (isEmailTaken)
			return CommandResult.Failure<RegisterPersonalUserResponse>(new ErrorResult
				{ StatusCode = HttpStatusCode.BadRequest, ErrorCode = "EmailAlreadyTaken" });
		var hash = hashService.HashPassword(command.Password);
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
		var isActivationEmailEnabled = await featureManager.IsEnabledAsync("UserEmailVerification");
		if (isActivationEmailEnabled)
			await codeGenerator.GenerateAndStoreCodeAsync(user);
		else
		{
			user.ActivateAccount();
		}

		await producer.Produce(new PersonalUserCreatedEvent
		{
			Email = user.Email,
			FirstName = user.PersonalDetails!.FirstName,
			LastName = user.PersonalDetails!.LastName,
			UserId = user.Id
		}, cancellationToken);

		await repository.SaveChangesAsync(cancellationToken);
		return CommandResult.Success(new RegisterPersonalUserResponse(isActivationEmailEnabled));
	}
}