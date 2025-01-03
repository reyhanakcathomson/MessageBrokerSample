using Infrastructure.MessageBroker;
using Infrastructure.MessageContracts;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;

public static partial class MassTransitExtensions
{
	public static IServiceCollection AddCustomMassTransitForProducer(this IServiceCollection services,
			MessageBrokerSettings messageBrokerSettings)
	{
		services.AddMassTransit(configure =>
				{
					configure.SetKebabCaseEndpointNameFormatter();
					configure.AddRequestClient<LedgerDataCancelRequest>();

					switch (messageBrokerSettings.Type)
					{
						case ServiceBusType.RabbitMQ:
							configure.UsingRabbitMq((context, cfg) =>
									{
										var settings = messageBrokerSettings.RabbitMq;
										cfg.Host(new Uri(settings.Host), h =>
										{
														h.Username(settings.Username);
														h.Password(settings.Password);
													});

										cfg.ConfigureEndpoints(context);

									});
							break;
						case ServiceBusType.AzureBus:
							configure.UsingAzureServiceBus(
										 (context, cfg) =>
										 {
											 cfg.Host(messageBrokerSettings.AzureServiceBus.ConnectionString);
											 cfg.ConfigureEndpoints(context);

										 });

							break;
						default:
							break;
					}
				});

		return services;
	}
}