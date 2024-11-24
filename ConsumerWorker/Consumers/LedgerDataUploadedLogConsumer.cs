using Infrastructure.MessageContracts;
using MassTransit;

namespace ConsumerWorker.Consumers;

public sealed class LedgerDataUploadedLogConsumer : IConsumer<LedgerDataUploaded>
{
    private readonly ILogger<LedgerDataUploadedLogConsumer> _logger;

    public LedgerDataUploadedLogConsumer(ILogger<LedgerDataUploadedLogConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<LedgerDataUploaded> context)
    {
        _logger.LogInformation($"Received LedgerDataUploadedLogConsumer: {context.Message.FileName}");
        return Task.CompletedTask;
    }
}