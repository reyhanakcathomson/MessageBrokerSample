using Infrastructure.Consumers;
using Infrastructure.MessageBroker;
using Infrastructure.MessageContracts;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Infrastructure.Extensions;

public static class MassTransitExtensions
{
    public static IServiceCollection AddCustomMassTransit(this IServiceCollection services)
    {
        services.AddMassTransit(configure =>
        {
            configure.AddConsumer<LedgerDataUploadedSendEmailConsumer>();
            configure.AddConsumer<LedgerDataUploadedLogConsumer>();
            configure.AddConsumer<LedgerDataAnalyzeConsumer>();

            configure.SetKebabCaseEndpointNameFormatter();

            configure.UsingRabbitMq((context, cfg) =>
            {
                var settings = context.GetRequiredService<MessageBrokerSettings>();

                cfg.Host(new Uri(settings.Host), h =>
                {
                    h.Username(settings.Username);
                    h.Password(settings.Password);
                });
                cfg.Publish<LedgerDataUploaded>(p => { p.ExchangeType = ExchangeType.Fanout; });
                cfg.ReceiveEndpoint("ledger-data-uploaded-log",
                    e => { e.ConfigureConsumer<LedgerDataUploadedLogConsumer>(context); });
                cfg.ReceiveEndpoint("ledger-data-uploaded-send-email",
                    e => { e.ConfigureConsumer<LedgerDataUploadedSendEmailConsumer>(context); });
                cfg.ReceiveEndpoint("ledger-data-analyze",
                    e => { e.ConfigureConsumer<LedgerDataAnalyzeConsumer>(context); });
            });
        });

        return services;
    }
}