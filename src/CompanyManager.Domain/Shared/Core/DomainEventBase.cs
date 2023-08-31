using CompanyManager.Domain.Shared.Contracts;

namespace CompanyManager.Domain.Shared.Core;

public class DomainEventBase : IDomainEvent
{
    protected DomainEventBase()
    {
        OccurredOn = TimeProvider.Now;
    }

    public DateTime OccurredOn { get; }
}