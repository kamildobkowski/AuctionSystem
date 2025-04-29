namespace Shared.Events.EventBus.Kafka;

internal class KafkaEventHandlerDefinition(Type handlerType, Type messageType, string topic)
{
	public Type HandlerType { get; } = handlerType;
	public Type MessageType { get; } = messageType;
	public string Topic { get; } = topic;
}