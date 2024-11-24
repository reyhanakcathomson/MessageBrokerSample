using ConsumerWorker;
using ConsumerWorker.Consumers;
using Infrastructure.MessageBroker;
using MassTransit;
using Microsoft.Extensions.Options;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureAppConfiguration((hostingContext, config) =>
{
    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
});

builder.ConfigureServices((hostContext, services) =>
{
    services.Configure<MessageBrokerSettings>(hostContext.Configuration.GetSection("MessageBroker"));

    services.AddSingleton(sp => sp.GetRequiredService<IOptions<MessageBrokerSettings>>().Value);

    services.AddMassTransit(configure =>
    {
        configure.AddConsumer<LedgerDataUploadedSendEmailConsumer>();
        configure.AddConsumer<LedgerDataUploadedLogConsumer>();
        configure.AddConsumer<LedgerDataAnalyzeConsumer>();

        configure.SetKebabCaseEndpointNameFormatter();

        configure.UsingRabbitMq((context, cfg) =>
        {
            MessageBrokerSettings settings = context.GetRequiredService<MessageBrokerSettings>();
            cfg.Host(new Uri(settings.Host),h =>
            {
                h.Username(settings.Username);
                h.Password(settings.Password);
            });

            cfg.ReceiveEndpoint("ledger-data-uploaded-log",
                e => { e.ConfigureConsumer<LedgerDataUploadedLogConsumer>(context); });
            cfg.ReceiveEndpoint("ledger-data-uploaded-send-email",
                e => { e.ConfigureConsumer<LedgerDataUploadedSendEmailConsumer>(context); });
            cfg.ReceiveEndpoint("ledger-data-analyze",
                e => { e.ConfigureConsumer<LedgerDataAnalyzeConsumer>(context); });

        });
    });

    services.AddHostedService<ConsumerService>();
});

await builder.Build().RunAsync();


