using CompanyManager.Domain.Shared.Contracts;
using CompanyManager.Domain.Shared.Core;

namespace CompanyManager.Domain.SystemLogs;

public class SystemLog<TEvent, TResource> : IAuditable where TEvent : IDomainEvent<TResource> where TResource : Entity
{
    private SystemLog(TEvent @event)
    {
        CreatedAt = @event.CreatedAt;
        Comment = @event.Comment;
        ResourceType = @event.ResourceType;
        Entity = @event.Entity ?? throw new ArgumentNullException(nameof(@event.Entity));
        Event = @event.EventAction;
    }

    public string Comment { get; }
    public string Event { get; }
    public string ResourceType { get; }
    public TResource Entity { get; }
    public DateTime CreatedAt { get; }

    public static SystemLog<TEvent, TResource> CreateNew(TEvent @event)
    {
        return new SystemLog<TEvent, TResource>(@event);
    }
}