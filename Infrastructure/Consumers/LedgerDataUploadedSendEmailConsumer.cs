using System.Text.Json;
using Infrastructure.MessageContracts;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Consumers;

public sealed class LedgerDataUploadedSendEmailConsumer : IConsumer<LedgerDataUploadedTopicMessage>
{
    private readonly ILogger<LedgerDataUploadedSendEmailConsumer> _logger;

    public LedgerDataUploadedSendEmailConsumer(ILogger<LedgerDataUploadedSendEmailConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<LedgerDataUploadedTopicMessage> context)
    {
        _logger.LogInformation($"File Uploaded email sent => {JsonSerializer.Serialize(context.Message)}");
        return Task.CompletedTask;
    }
}



