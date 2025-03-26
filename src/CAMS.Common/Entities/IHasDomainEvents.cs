namespace CAMS.Common.Entities;

/// <summary>
/// Defines a contract for entities that raise domain events.
/// </summary>
public interface IHasDomainEvents
{
    List<DomainEvent> DomainEvents { get; }
}
