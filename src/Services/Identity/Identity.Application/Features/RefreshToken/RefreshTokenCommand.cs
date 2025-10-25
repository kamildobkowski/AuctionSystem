using Identity.Domain.Services;
using Shared.Base.Cqrs.Commands;

namespace Identity.Application.Features.RefreshToken;

public record RefreshTokenCommand(string Token) : ICommand;

internal class RefreshTokenCommandHandler(ITokenService tokenService) 
	: ICommandHandler<RefreshTokenCommand, RefreshTokenResponse>
{
	public async Task<ICommandResult<RefreshTokenResponse>> HandleAsync(RefreshTokenCommand command, CancellationToken cancellationToken = default)
	{
		var result = await tokenService.RefreshAsync(command.Token);
		if (!result.IsSuccess)
			return CommandResult.Failure<RefreshTokenResponse>(result.ErrorResult!);
		return CommandResult.Success(new RefreshTokenResponse(result.Value.AccessToken, result.Value.RefreshToken));
	}
}