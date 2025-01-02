using System.Text.Json;
using Infrastructure.MessageContracts;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Consumers;

public sealed class LedgerDataUploadedLogConsumer : IConsumer<LedgerDataUploadedTopicMessage>
{
	private readonly ILogger<LedgerDataUploadedLogConsumer> _logger;

	public LedgerDataUploadedLogConsumer(ILogger<LedgerDataUploadedLogConsumer> logger)
	{
		_logger = logger;
	}

	public Task Consume(ConsumeContext<LedgerDataUploadedTopicMessage> context)
	{
		ArgumentNullException.ThrowIfNull(context, nameof(context));

		_logger.LogDebug($"LedgerDataUploadedLogConsumer consumed the messageId:{context.MessageId}");
		_logger.LogInformation($"File Uploaded logged => {JsonSerializer.Serialize(context.Message)}");
		return Task.CompletedTask;
	}
}