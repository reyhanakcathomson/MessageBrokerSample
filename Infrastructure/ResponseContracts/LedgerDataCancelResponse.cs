namespace Infrastructure.ResponseContracts
{
    public class LedgerDataCancelResponse
    {
        public int LedgerDataId { get; set; }
        public DateTime CanceledDate { get; set; }
    }
}
