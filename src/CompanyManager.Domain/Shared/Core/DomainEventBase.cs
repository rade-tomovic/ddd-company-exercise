using CompanyManager.Domain.Shared.Contracts;

namespace CompanyManager.Domain.Shared.Core;

public abstract class DomainEventBase : IDomainEvent<Entity>
{
    public DateTime CreatedAt { get; } = TimeProvider.Now;
    public string Comment { get; init; }
    public string ResourceType { get; init; }
    public string EventAction { get; init; }
    public Entity Entity { get; init; }
}