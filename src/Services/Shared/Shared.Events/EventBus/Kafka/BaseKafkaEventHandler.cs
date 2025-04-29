using Confluent.Kafka;

namespace Shared.Events.EventBus.Kafka;

public abstract class BaseKafkaEventHandler<TKey, TValue>
{
	private readonly string _bootstrapServers;
	private readonly string _topic;
	private readonly string _groupId;
	
	protected BaseKafkaEventHandler(string bootstrapServers, string topic, string groupId)
	{
		_bootstrapServers = bootstrapServers;
		_topic = topic;
		_groupId = groupId;
	}
	
	public async Task StartAsync(CancellationToken cancellationToken)
	{
		var config = new ConsumerConfig
		{
			BootstrapServers = _bootstrapServers,
			GroupId = _groupId,
			AutoOffsetReset = AutoOffsetReset.Earliest,
			EnableAutoCommit = true,
		};

		using var consumer = new ConsumerBuilder<TKey, TValue>(config)
			.SetErrorHandler((_, e) => Console.WriteLine($"Błąd: {e.Reason}"))
			.Build();

		consumer.Subscribe(_topic);
		Console.WriteLine($"Subskrybuję temat: {_topic}");

		try
		{
			while (!cancellationToken.IsCancellationRequested)
			{
				try
				{
					var consumeResult = consumer.Consume(cancellationToken);
					if (consumeResult != null)
					{
						await HandleEventAsync(consumeResult.Message.Key, consumeResult.Message.Value,
							cancellationToken);
					}
				}
				catch (ConsumeException ex)
				{
					Console.WriteLine($"Błąd odbioru wiadomości: {ex.Error.Reason}");
				}
			}
		}
		catch (OperationCanceledException)
		{
			Console.WriteLine("Zatrzymano konsumenta.");
		}
		finally
		{
			consumer.Close();
		}
	}
	
	protected abstract Task HandleEventAsync(TKey key, TValue value, CancellationToken cancellationToken);
}