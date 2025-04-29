namespace Shared.Events.EventBus;

public interface IEvent
{
	static abstract string Topic { get; }
}