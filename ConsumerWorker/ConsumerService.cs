using MassTransit;

namespace ConsumerWorker;
 
public class ConsumerService : BackgroundService
{
    private readonly IBusControl _busControl;

    public ConsumerService(IBusControl busControl)
    {
        _busControl = busControl;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();
        

        await _busControl.StartAsync(stoppingToken);
        try
        {
            Console.WriteLine("Consumer started. Listening for messages...");
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        finally
        {
            await _busControl.StopAsync(stoppingToken);
        }
    }
}