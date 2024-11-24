using Infrastructure.Abstractions;
using Infrastructure.Extensions;
using Infrastructure.MessageBroker;
using Infrastructure.MessageContracts;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<MessageBrokerSettings>(builder.Configuration.GetSection("MessageBroker"));

builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<MessageBrokerSettings>>().Value);

builder.Services.AddCustomMassTransit();

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
        await eventBus.PublishAsync(new LedgerDataUploaded
            { FileName = fileName, UploadedDate = DateTime.Now.ToUniversalTime() });
        return Results.Ok();
    })
    .WithName("upload-ledger-data")
    .WithOpenApi();
app.Run();