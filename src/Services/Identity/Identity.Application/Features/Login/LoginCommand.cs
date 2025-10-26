using FluentValidation;
using Identity.Domain.Repositories;
using Identity.Domain.Services;
using Shared.Base.Cqrs.Commands;
using Shared.Base.Errors;

namespace Identity.Application.Features.Login;

public record LoginCommand(string Email, string Password) : ICommand;

internal class LoginCommandHandler(
	IValidator<LoginCommand> validator, 
	IHashService hashService, 
	IUserRepository userRepository, 
	ITokenService tokenService)
	: ICommandHandler<LoginCommand, LoginResponse>
{
	public async Task<ICommandResult<LoginResponse>> HandleAsync(LoginCommand command, CancellationToken cancellationToken = default)
	{
		var result = await validator.ValidateAsync(command, cancellationToken);
		if (!result.IsValid)
			return CommandResult.Failure<LoginResponse>(ErrorResult.ValidationError(result));
		
		var user = await userRepository.Get(x => x.Email == command.Email);
		if (user is null)
			return CommandResult.Failure<LoginResponse>(new ErrorResult());
		var isPasswordCorrect = hashService.VerifyHash(user, command.Password);
		if (!isPasswordCorrect)
			return CommandResult.Failure<LoginResponse>(new ErrorResult());
		var accessToken = tokenService.GenerateAccessToken(user);
		var refreshToken = await tokenService.GenerateRefreshToken(user);
		return CommandResult.Success(new LoginResponse(accessToken.Token, accessToken.Expires, refreshToken.Token, refreshToken.Expires));
	}
}