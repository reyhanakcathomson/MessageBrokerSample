using MassTransit;
using IEventBus = Infrastructure.Abstractions.IEventBus;

namespace Infrastructure.MessageBroker;

public sealed class EventBus :IEventBus
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IBus _bus;

    public EventBus(IPublishEndpoint publishEndpoint, IBus bus)
    {
        _publishEndpoint = publishEndpoint;
        _bus = bus;
    }
    public async Task SendAsync<T>(string destinationUrl,T message, CancellationToken cancellationToken = default)
    {
        if (message == null)
            throw new ArgumentNullException(nameof(message));

        var sendEndpoint = await _bus.GetSendEndpoint(new Uri(destinationUrl));

        await sendEndpoint.Send(message, cancellationToken);
    }
    public Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) 
        where T:class => _publishEndpoint.Publish(message, cancellationToken);
}