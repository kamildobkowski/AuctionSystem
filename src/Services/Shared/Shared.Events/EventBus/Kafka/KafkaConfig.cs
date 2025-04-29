using Confluent.Kafka;

namespace Shared.Events.EventBus.Kafka;

public class KafkaConfig
{
	public required string BootstrapServers { get; init; }
	public required string GroupId { get; init; } = default!;
	public AutoOffsetReset AutoOffsetReset { get; set; } = AutoOffsetReset.Earliest;
}