using Infrastructure.MessageContracts;
using MassTransit;

namespace ConsumerWorker.Consumers;

public sealed class LedgerDataAnalyzeConsumer : IConsumer<LedgerDataUploaded>
{
    private readonly ILogger<LedgerDataAnalyzeConsumer> _logger;

    public LedgerDataAnalyzeConsumer(ILogger<LedgerDataAnalyzeConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<LedgerDataUploaded> context)
    {
        _logger.LogInformation($"Received LedgerDataAnalyzeConsumer: {context.Message.FileName}");
        return Task.CompletedTask;
    }
}