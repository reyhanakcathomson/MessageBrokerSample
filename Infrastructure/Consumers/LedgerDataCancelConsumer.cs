using Infrastructure.Entities;
using Infrastructure.MessageContracts;
using Infrastructure.ResponseContracts;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Consumers;


public class LedgerDataCancelConsumer :
		IConsumer<LedgerDataCancelRequest>
{
	private readonly ILedgerDataRepository _ledgerDataRepository;
	private readonly ILogger<LedgerDataCancelConsumer> _logger;

	public LedgerDataCancelConsumer(ILedgerDataRepository ledgerDataRepository,
		ILogger<LedgerDataCancelConsumer> logger)
	{
		_ledgerDataRepository = ledgerDataRepository;
		_logger = logger;
	}
	public async Task Consume(ConsumeContext<LedgerDataCancelRequest> context)
	{
		ArgumentNullException.ThrowIfNull(context, nameof(context));
		_logger.LogDebug($"LedgerDataCancelConsumer consumed the messageId:{ context.MessageId}");

		var ledgerData = await _ledgerDataRepository.Get(context.Message.LedgerDataId);
		if (ledgerData == null)
		{
			await context.RespondAsync<LedgerDataNotFound>(context.Message);
			return;
		}

		if (ledgerData.Status == LedgerDataStatus.Canceled)
		{
			await context.RespondAsync<LedgerDataAlreadyCanceled>(context.Message);
			return;
		}

		ledgerData.Cancel();
		var response = new LedgerDataCancelResponse
		{
			LedgerDataId = ledgerData.Id,
			CanceledDate = ledgerData.CanceledDate
		};

		await context.RespondAsync(response);

	}
}
