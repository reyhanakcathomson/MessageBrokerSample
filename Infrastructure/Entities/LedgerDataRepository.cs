namespace Infrastructure.Entities
{
    public interface ILedgerDataRepository
    {
      Task<LedgerData> Get(int id);
    }
    public class LedgerDataRepository: ILedgerDataRepository
    {
        public async Task<LedgerData> Get(int id)
        {
            if (id  == 0)
            {
                return null;

            }
            if (id  == 1)
            {
                return new LedgerData()
                {
                    Id = id,
                    CreatedDate = DateTime.Now.Subtract(TimeSpan.FromHours(-10)),
                    Status = LedgerDataStatus.Canceled
                };

            }
            if (id % 2 == 0)
            {
                return new LedgerData()
                {
                    Id = id,
                    CreatedDate = DateTime.Now.Subtract(TimeSpan.FromHours(-10)),
                    Status = LedgerDataStatus.Processing
                };
           
            }

            return new LedgerData()
            {
                Id = id,
                CreatedDate = DateTime.Now.Subtract(TimeSpan.FromHours(-20)),
                Status = LedgerDataStatus.Requested,
                ProcessedDate = DateTime.Now.Subtract(TimeSpan.FromHours(-5))
            };

        }
    }
}
