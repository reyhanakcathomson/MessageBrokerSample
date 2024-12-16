namespace Infrastructure.ResponseContracts
{
    public class LedgerDataNotFound
    {
        public int LedgerDataId { get; set; }
    }

    public class LedgerDataAlreadyCanceled
    {
        public int LedgerDataId { get; set; }
        public DateTime CanceledDate { get; set; }
    }
}
