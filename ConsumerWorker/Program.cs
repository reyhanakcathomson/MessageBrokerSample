using ConsumerWorker;
using Infrastructure.Extensions;
using Infrastructure.MessageBroker;
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

    services.AddCustomMassTransit();

    services.AddHostedService<ConsumerService>();
});

await builder.Build().RunAsync();


