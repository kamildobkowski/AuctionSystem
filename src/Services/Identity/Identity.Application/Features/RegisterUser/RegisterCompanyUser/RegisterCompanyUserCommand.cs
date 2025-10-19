using System.Net;
using FluentValidation;
using Identity.Application.Features.RegisterUser.Common.Models;
using Identity.Domain.Entities;
using Identity.Domain.Repositories;
using Identity.Domain.Services;
using Identity.Domain.ValueObjects;
using Microsoft.FeatureManagement;
using Shared.Base.Cqrs.Commands;
using Shared.Base.Errors;

namespace Identity.Application.Features.RegisterUser.RegisterCompanyUser;

public sealed record RegisterCompanyUserCommand(string Email, 
	string Name, 
	string TaxId, 
	AddressModel Address, 
	string PhonePrefix, 
	string PhoneNumber, 
	string Password, 
	string RepeatPassword) 
	: ICommand;

internal sealed class RegisterCompanyUserCommandHandler(IValidator<RegisterCompanyUserCommand> validator, ICacheService cacheService, 
	IHashService hashService, IUserRepository repository, IFeatureManager featureManager, IActivationCodeGenerator codeGenerator)
	: ICommandHandler<RegisterCompanyUserCommand, RegisterCompanyUserResponse>
{
	public async Task<ICommandResult<RegisterCompanyUserResponse>> HandleAsync(
		RegisterCompanyUserCommand command, 
		CancellationToken cancellationToken = default)
	{
		var result = await validator.ValidateAsync(command, cancellationToken);
		if (!result.IsValid)
			return CommandResult.Failure<RegisterCompanyUserResponse>(ErrorResult.ValidationError(result));
		
		var isEmailTaken = await cacheService.IsEmailTaken(command.Email);
		if (isEmailTaken)
			return CommandResult.Failure<RegisterCompanyUserResponse>(new ErrorResult
				{ StatusCode = HttpStatusCode.BadRequest, ErrorCode = "EmailAlreadyTaken" });
		var hash = hashService.HashPassword(command.Password);
		var user = User.CreateCompany(
			email: command.Email, 
			passwordHash: hash, 
			companyName: command.Name, 
			nip: command.TaxId, 
			address: command.Address.ToEntity(),
			phoneNumber: new PhoneNumber
			{
				Prefix = command.PhonePrefix,
				Number = command.PhoneNumber
			});
		
		repository.Add(user);
		await repository.SaveChangesAsync(cancellationToken);
		var isActivationEmailEnabled = await featureManager.IsEnabledAsync("UserEmailVerification");
		if (isActivationEmailEnabled)
			await codeGenerator.GenerateAndStoreCodeAsync(user);
		else
		{
			user.ActivateAccount();
			await repository.SaveChangesAsync(cancellationToken);
		}
		
		return CommandResult.Success(new RegisterCompanyUserResponse(isActivationEmailEnabled));
	}
}