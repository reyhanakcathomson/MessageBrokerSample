using Infrastructure.Consumers;
using Infrastructure.MessageBroker;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;

public static partial class MassTransitExtensions
{
    public static IServiceCollection AddCustomMassTransitForConsumer(this IServiceCollection services,
        MessageBrokerSettings messageBrokerSettings)
    {


        services.AddMassTransit(configure =>
        {
            configure.AddServiceBusMessageScheduler();

            configure.AddConsumer<LedgerDataUploadedSendEmailConsumer>();
            configure.AddConsumer<LedgerDataUploadedLogConsumer>();
            configure.AddConsumer<LedgerDataAnalyzeConsumer>(typeof(LedgerDataAnalyzeConsumerDefinition));
            configure.AddConsumer<LedgerDataCancelConsumer>();

            configure.SetKebabCaseEndpointNameFormatter();

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

                      /*  cfg.Publish<LedgerDataUploaded>(p => { p.ExchangeType = ExchangeType.Direct; });
                        cfg.ReceiveEndpoint("ledger-data-uploaded-log",
                            e => { e.ConfigureConsumer<LedgerDataUploadedLogConsumer>(context); });
                        cfg.ReceiveEndpoint("ledger-data-uploaded-send-email",
                            e => { e.ConfigureConsumer<LedgerDataUploadedSendEmailConsumer>(context); });
                        cfg.ReceiveEndpoint(MessageBrokerConstants.LedgerDataAnalyzeQueue,
                            e => { e.ConfigureConsumer<LedgerDataAnalyzeConsumer>(context); });*/
                    });
                    break;
                case ServiceBusType.AzureBus:
                    configure.UsingAzureServiceBus(
                       (context, cfg) =>
                       {
                           cfg.Host(messageBrokerSettings.AzureServiceBus.ConnectionString);
                           cfg.UseServiceBusMessageScheduler();

                           cfg.UseMessageRetry(retry => retry.Interval(2, TimeSpan.FromSeconds(3)));

                           cfg.ConfigureEndpoints(context);

/*
                           cfg.Message<LedgerDataUploadedTopicMessage>(m => m.SetEntityName(MessageBrokerConstants.LedgerDataUploadedTopic));
                           cfg.SubscriptionEndpoint<LedgerDataUploadedTopicMessage>(MessageBrokerConstants.LedgerDataUploadedTopic, configurator =>
                           {
                               configurator.ConfigureConsumer<LedgerDataUploadedSendEmailConsumer>(context);
                               configurator.ConfigureConsumer<LedgerDataUploadedLogConsumer>(context);
                           });
                           cfg.ReceiveEndpoint(MessageBrokerConstants.LedgerDataAnalyzeQueue,
                               e => { e.ConfigureConsumer<LedgerDataAnalyzeConsumer>(context); });*/
                       });

                    break;
                default:
                    break;
            }


        });

        return services;
    }
}