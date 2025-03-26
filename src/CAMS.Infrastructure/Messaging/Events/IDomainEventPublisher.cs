using CAMS.Common.Entities;

namespace CAMS.Infrastructure.Messaging.Events;

/// <summary>
/// Defines a contract for publishing domain events.
/// </summary>
public interface IDomainEventPublisher
{
    Task PublishEventsAsync<T>(T aggregate) where T : IHasDomainEvents;
    Task SendMessageAsync<T>(T message);
}


