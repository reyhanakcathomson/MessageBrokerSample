using Infrastructure.Abstractions;
using Infrastructure.Extensions;
using Infrastructure.MessageBroker;
using Infrastructure.MessageContracts;
using Infrastructure.ResponseContracts;
using MassTransit;
using System.Text.Json;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();


        MessageBrokerSettings messageBrokerSettings = builder.Configuration.GetSection("MessageBroker").Get<MessageBrokerSettings>()
            ?? throw new ArgumentNullException(nameof(MessageBrokerSettings));
        builder.Services.AddCustomMassTransitForProducer(messageBrokerSettings);

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
                await eventBus.SendAsync($"{MessageBrokerConstants.LedgerDataAnalyzeQueue}", new LedgerDataUploaded
                { FileName = fileName, UploadedDate = DateTime.Now.ToUniversalTime() });

                await eventBus.PublishAsync(new LedgerDataUploadedTopicMessage
                { FileName = fileName, UploadedDate = DateTime.Now.ToUniversalTime(), Info1 = "test1" });
                return Results.Ok();
            })
            .WithName("upload-ledger-data")
            .WithOpenApi();

        app.MapPost("/cancel-ledger-data", async (int fileId, IRequestClient<LedgerDataCancelRequest> _client, CancellationToken cancellationToken) =>
        {
            var response = await _client.GetResponse<LedgerDataCancelResponse, LedgerDataAlreadyCanceled, LedgerDataNotFound>
            (new LedgerDataCancelRequest { LedgerDataId = fileId }, cancellationToken);
            if (response.Is(out Response<LedgerDataCancelResponse> accepted))
            {
                return Results.Ok(accepted);
            }
            else
            {
                return Results.BadRequest(JsonSerializer.Serialize(response.Message));
            }


        })
            .WithName("cancel-ledger-data")
            .WithOpenApi();

        app.Run();
    }
}