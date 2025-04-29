namespace Shared.Events.EventBus;

public interface IEventHandler<in T>
where T : IEvent
{
	/// <summary>
	/// Called for each message of type T pulled from Event Bus.
	/// </summary>
	Task HandleAsync(T message, CancellationToken cancellationToken);
}