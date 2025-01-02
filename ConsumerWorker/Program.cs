using ConsumerWorker;
using Infrastructure.Entities;
using Infrastructure.Extensions;
using Infrastructure.MessageBroker;
using Serilog;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureAppConfiguration((hostingContext, config) =>
{
	config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
});


builder.ConfigureServices((hostContext, services) =>
{
	services.AddSerilog(srl =>
	{
		srl.MinimumLevel.Debug()
			 .WriteTo.Console();
	});
	MessageBrokerSettings messageBrokerSettings = hostContext.Configuration.GetSection("MessageBroker").Get<MessageBrokerSettings>()
		?? throw new ArgumentNullException(nameof(MessageBrokerSettings));
	services.AddScoped<ILedgerDataRepository, LedgerDataRepository>();

	services.AddCustomMassTransitForConsumer(messageBrokerSettings);

	services.AddHostedService<ConsumerService>();
});

await builder.Build().RunAsync();


