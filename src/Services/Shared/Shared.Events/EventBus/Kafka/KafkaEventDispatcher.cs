using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Shared.Events.EventBus.Kafka;

public class KafkaEventDispatcher : BackgroundService
{
	private readonly IServiceProvider _sp;
	private readonly ILogger<KafkaEventDispatcher> _logger;
	private readonly KafkaConfig _kafkaConfig;
	private readonly IReadOnlyCollection<KafkaEventHandlerDefinition> _defs;

	internal KafkaEventDispatcher(
		IServiceProvider serviceProvider,
		IOptions<KafkaConfig> kafkaConfig,
		IEnumerable<KafkaEventHandlerDefinition> definitions,
		ILogger<KafkaEventDispatcher> logger)
	{
		_sp = serviceProvider;
		_logger = logger;
		_kafkaConfig = kafkaConfig.Value;
		_defs = definitions.ToList();
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		var consumerConfig = new ConsumerConfig
		{
			BootstrapServers = _kafkaConfig.BootstrapServers,
			GroupId = _kafkaConfig.GroupId,
			AutoOffsetReset = _kafkaConfig.AutoOffsetReset
		};
		using var consumer = new ConsumerBuilder<Ignore, string>(consumerConfig)
			.SetErrorHandler((_, e) => _logger.LogError("Kafka error: {reason}", e.Reason))
			.Build();

		var topics = _defs.Select(d => d.Topic).Distinct();
		consumer.Subscribe(topics);

		while (!stoppingToken.IsCancellationRequested)
		{
			try
			{
				var cr = consumer.Consume(stoppingToken);
				if (cr?.Message == null) continue;

				var json = cr.Message.Value;
				var topic = cr.Topic;

				using var scope = _sp.CreateScope();
				foreach (var def in _defs.Where(d => d.Topic == topic))
				{
					var handler = scope.ServiceProvider.GetRequiredService(def.HandlerType);

					var msg = JsonSerializer.Deserialize(json, def.MessageType,
						new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

					var mi = def.HandlerType.GetMethod(
						"HandleAsync",
						new[] { def.MessageType, typeof(CancellationToken) }
					);
					if (mi is null)
						throw new InvalidOperationException(
							$"Handler {def.HandlerType.Name} has no HandleAsync method.");

					await (Task) mi.Invoke(handler, [ msg, stoppingToken ])!;
				}
			}
			catch (ConsumeException ex)
			{
				_logger.LogError("Consume error: {reason}", ex.Error.Reason);
			}
			catch (OperationCanceledException)
			{
				break;
			}
			catch (Exception ex)
			{
				_logger.LogError("Dispatching exception: {exception}", ex);
			}
		}

		consumer.Close();
	}
}