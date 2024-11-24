using MassTransit;
using IEventBus = Infrastructure.Abstractions.IEventBus;

namespace Infrastructure.MessageBroker;

public sealed class EventBus :IEventBus
{
    private readonly IPublishEndpoint _publishEndpoint;

    public EventBus(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }
    public Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) 
        where T:class => _publishEndpoint.Publish(message, cancellationToken);
}