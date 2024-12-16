using Infrastructure.Entities;
using Infrastructure.MessageContracts;
using Infrastructure.ResponseContracts;
using MassTransit;

namespace Infrastructure.Consumers;


public class LedgerDataCancelConsumer :
    IConsumer<LedgerDataCancelRequest>
{
    private readonly ILedgerDataRepository _ledgerDataRepository;

    public LedgerDataCancelConsumer(ILedgerDataRepository ledgerDataRepository)
    {
        _ledgerDataRepository = ledgerDataRepository;
    }
    public async Task Consume(ConsumeContext<LedgerDataCancelRequest> context)
    {
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
