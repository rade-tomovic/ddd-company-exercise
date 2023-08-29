using CompanyManager.Domain.Shared.Contracts;

namespace CompanyManager.Domain.Shared.Core;

public class DomainEventBase : IDomainEvent
{
    public DomainEventBase(DateTime occurredOn)
    {
        OccurredOn = occurredOn;
    }

    public DateTime OccurredOn { get; }
}