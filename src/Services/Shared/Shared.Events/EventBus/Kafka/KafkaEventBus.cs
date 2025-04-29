using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Shared.Events.EventBus.Kafka;

public class KafkaEventBus : IEventBus
{
	private readonly ILogger<KafkaEventBus> _logger;
	private readonly IProducer<Null, string> _producer;
	private readonly JsonSerializerOptions    _jsonOptions;

	public KafkaEventBus(IOptions<KafkaConfig> configOptions, ILogger<KafkaEventBus> logger)
	{
		_logger = logger;
		var cfg = new ProducerConfig
		{
			BootstrapServers = configOptions.Value.BootstrapServers,
			Acks = Acks.All
		};

		_producer = new ProducerBuilder<Null, string>(cfg)
			.SetErrorHandler((_, e) =>
				Console.WriteLine($"Kafka producer error: {e.Reason}")
			)
			.Build();

		_jsonOptions = new JsonSerializerOptions
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		};
	}

	public async Task PublishAsync<TEvent>(
		TEvent @event,
		string topic,
		CancellationToken cancellationToken = default) 
		where TEvent : IEvent
	{
		var json = JsonSerializer.Serialize(@event, _jsonOptions);

		var msg = new Message<Null, string> { Value = json };
		var delivery = await _producer.ProduceAsync(topic, msg, cancellationToken);

		_logger.LogInformation(
			$"Published {typeof(TEvent).Name} to {delivery.TopicPartitionOffset}"
		);
	}

	public Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) 
		where TEvent : IEvent
		=> PublishAsync(@event, TEvent.Topic, cancellationToken);

	public void Dispose()
	{
		_producer.Flush(TimeSpan.FromSeconds(5));
		_producer.Dispose();
		GC.SuppressFinalize(this);
	}
}