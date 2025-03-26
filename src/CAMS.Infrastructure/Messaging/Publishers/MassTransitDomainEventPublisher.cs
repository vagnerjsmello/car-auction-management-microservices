using CAMS.Common.Entities;
using CAMS.Infrastructure.Messaging.Events;
using MassTransit;

namespace CAMS.Infrastructure.Events.MassTransit;

public class MassTransitDomainEventPublisher : IDomainEventPublisher
{
    private readonly IBus _bus;

    public MassTransitDomainEventPublisher(IBus bus)
    {
        _bus = bus;
    }

    public async Task PublishEventsAsync<T>(T aggregate) where T : IHasDomainEvents
    {
        foreach (var domainEvent in aggregate.DomainEvents)
        {
            // Publish the domain event
            await _bus.Publish(domainEvent);
        }

        // Clear the events to avoid re-publishing.
        aggregate.DomainEvents.Clear();
    }

    // Explicit interface implementation for SendMessageAsync
    async Task IDomainEventPublisher.SendMessageAsync<T>(T message)
    {
        await _bus.Send(message);
    }
}
