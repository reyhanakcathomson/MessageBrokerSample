namespace Infrastructure.MessageContracts;

public class LedgerDataUploaded
{
    public required string FileName { get; set; }
    public DateTime UploadedDate { get; set; }
}