namespace Infrastructure.MessageContracts
{
    public sealed class LedgerDataUploadedTopicMessage
    {
        public required string FileName { get; set; }
        public DateTime UploadedDate { get; set; }
        public string Info1 { get; set; }
    }
}
