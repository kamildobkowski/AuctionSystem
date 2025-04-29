namespace Shared.Events.EventBus;

public interface IEventBus : IDisposable
{
	Task PublishAsync<TEvent>(TEvent @event, string topic, 
		CancellationToken cancellationToken = default) where TEvent : IEvent;
	
	Task PublishAsync<TEvent>(TEvent @event, 
		CancellationToken cancellationToken = default) where TEvent : IEvent;
}