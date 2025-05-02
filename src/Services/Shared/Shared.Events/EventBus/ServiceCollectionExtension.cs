using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Shared.Events.EventBus.Kafka;

namespace Shared.Events.EventBus;

public static class ServiceCollectionExtension
{
	public static IServiceCollection AddEventHandler<TEvent, THandler>(this IServiceCollection services, string topic) 
		where TEvent : IEvent
		where THandler : class, IEventHandler<TEvent>
	{
		services.AddScoped<THandler>();
		services.AddSingleton(new KafkaEventHandlerDefinition(typeof(THandler), typeof(TEvent), topic));
		services.TryAddSingleton<KafkaEventDispatcher>();
		services.TryAddEnumerable(ServiceDescriptor.Singleton<IHostedService>(sp 
			=> sp.GetRequiredService<KafkaEventDispatcher>()));
		return services;
	}

	public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
	{
		var kafkaConfig =
			configuration.GetSection("Kafka").Get<KafkaConfig>() 
			?? throw new MissingFieldException("Kafka config is missing");
		services.AddSingleton<KafkaConfig>(kafkaConfig);
		services.AddSingleton<IEventBus, KafkaEventBus>();
		return services;
	}
}