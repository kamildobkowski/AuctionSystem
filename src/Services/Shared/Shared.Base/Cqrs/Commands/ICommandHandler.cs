namespace Shared.Base.Cqrs.Commands;

public interface ICommandHandler<TCommand, TResult>
	where TCommand : ICommand
{
	Task<ICommandResult<TResult>> HandleAsync(TCommand command, CancellationToken cancellationToken = default);
}