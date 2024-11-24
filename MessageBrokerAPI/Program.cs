using Infrastructure.Abstractions;
using Infrastructure.MessageBroker;
using Infrastructure.MessageContracts;
using MassTransit;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<MessageBrokerSettings>(builder.Configuration.GetSection("MessageBroker"));

builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<MessageBrokerSettings>>().Value);

builder.Services.AddMassTransit(configure =>
{
    configure.SetKebabCaseEndpointNameFormatter();
    configure.UsingRabbitMq((context, cfg) =>
    {
        MessageBrokerSettings settings = context.GetRequiredService<MessageBrokerSettings>();
        cfg.Host(new Uri(settings.Host), h =>
        {
            h.Username(settings.Username);
            h.Password(settings.Password);
        });
        cfg.Publish<LedgerDataUploaded>(p => { p.ExchangeType = ExchangeType.Fanout; });
    });
});


builder.Services.AddTransient<IEventBus, EventBus>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/upload-ledger-data", async (string fileName, IEventBus eventBus) =>
    {
        await eventBus.PublishAsync(new LedgerDataUploaded { FileName = fileName });
        return Results.Ok();
    })
    .WithName("upload-ledger-data")
    .WithOpenApi();
app.Run();