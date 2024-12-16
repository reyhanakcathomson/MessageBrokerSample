
namespace Infrastructure.Entities
{
    public class LedgerData
    {
        public int Id { get; internal set; }
        public DateTime CreatedDate { get; internal set; }
        public LedgerDataStatus Status { get; internal set; }
        public DateTime CanceledDate { get; private set; }
        public DateTime ProcessedDate { get; internal set; }
        public void Cancel()
        {
            Status = LedgerDataStatus.Canceled;
            CanceledDate = DateTime.Now;
        }
    }
    public enum LedgerDataStatus
    {
        Requested=1,
        Uploaded=2,
        Processing = 3,
        Processed=4,
        Canceled=5
    }
}
