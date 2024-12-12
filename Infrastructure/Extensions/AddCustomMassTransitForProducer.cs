using Infrastructure.Consumers;
using Infrastructure.MessageBroker;
using Infrastructure.MessageContracts;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Infrastructure.Extensions;

public static partial class MassTransitExtensions
{
    public static IServiceCollection AddCustomMassTransitForProducer(this IServiceCollection services,
        MessageBrokerSettings messageBrokerSettings)
    {


        services.AddMassTransit(configure =>
        {
            configure.AddServiceBusMessageScheduler();          

            configure.SetKebabCaseEndpointNameFormatter();

            switch (messageBrokerSettings.Type)
            {
                case ServiceBusType.RabbitMQ:
                    configure.UsingRabbitMq((context, cfg) =>
                    {
                        cfg.UseServiceBusMessageScheduler();
                        cfg.UseMessageRetry(retry => retry.Interval(2, TimeSpan.FromSeconds(3)));

                        var settings = messageBrokerSettings.RabbitMq;
                        cfg.Host(new Uri(settings.Host), h =>
                        {
                            h.Username(settings.Username);
                            h.Password(settings.Password);
                        });
                       

                        cfg.Publish<LedgerDataUploaded>(p => { p.ExchangeType = ExchangeType.Direct; });                     
                    });
                    break;
                case ServiceBusType.AzureBus:
                    configure.UsingAzureServiceBus(
                       (context, cfg) =>
                       {
                           cfg.UseServiceBusMessageScheduler();
                           cfg.UseMessageRetry(retry => retry.Interval(2, TimeSpan.FromSeconds(3)));

                           cfg.Host(messageBrokerSettings.AzureServiceBus.ConnectionString);

                           //Create a topic for LedgerDataUploaded
                           cfg.Message<LedgerDataUploadedTopicMessage>(m => m.SetEntityName(MessageBrokerConstants.LedgerDataUploadedTopic));                        
                          
                       });

                    break;
                default:
                    break;
            }


        });

        return services;
    }
}