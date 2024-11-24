using Infrastructure.MessageContracts;
using MassTransit;

namespace ConsumerWorker.Consumers;

public sealed class LedgerDataUploadedSendEmailConsumer : IConsumer<LedgerDataUploaded>
{
    private readonly ILogger<LedgerDataUploadedSendEmailConsumer> _logger;

    public LedgerDataUploadedSendEmailConsumer(ILogger<LedgerDataUploadedSendEmailConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<LedgerDataUploaded> context)
    {
        _logger.LogInformation($"Received LedgerDataUploadedSendEmailConsumer: {context.Message.FileName}");
        return Task.CompletedTask;
    }
}