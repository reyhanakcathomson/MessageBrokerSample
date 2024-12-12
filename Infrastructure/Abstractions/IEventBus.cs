namespace Infrastructure.Abstractions;

public interface IEventBus
{
    Task SendAsync<T>(string destinationUrl, T message, CancellationToken cancellationToken = default);
    Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) where T : class;
}