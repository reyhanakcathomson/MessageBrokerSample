using System.Text.Json;
using Infrastructure.MessageContracts;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Consumers;

public sealed class LedgerDataAnalyzeConsumer : IConsumer<LedgerDataUploaded>
{
	private readonly ILogger<LedgerDataAnalyzeConsumer> _logger;

	public LedgerDataAnalyzeConsumer(ILogger<LedgerDataAnalyzeConsumer> logger)
	{
		_logger = logger;
	}

    public Task Consume(ConsumeContext<LedgerDataUploaded> context)
	{
		ArgumentNullException.ThrowIfNull(context, nameof(context));
		_logger.LogDebug($"LedgerDataAnalyzeConsumer consumed the messageId:{context.MessageId}");
		_logger.LogInformation($"Analyze started => {JsonSerializer.Serialize(context.Message)}");
		return Task.CompletedTask;
	}
}