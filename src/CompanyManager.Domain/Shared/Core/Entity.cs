using CompanyManager.Domain.Shared.Contracts;
using CompanyManager.Domain.Shared.Exceptions;

namespace CompanyManager.Domain.Shared.Core;

public class Entity
{
    private List<IDomainEvent<Entity>> _domainEvents;

    public IReadOnlyCollection<IDomainEvent<Entity>>? DomainEvents => _domainEvents?.AsReadOnly();

    protected void AddDomainEvent(IDomainEvent<Entity> domainEvent)
    {
        _domainEvents ??= new List<IDomainEvent<Entity>>();
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents?.Clear();
    }

    protected static void CheckRule(IBusinessRule rule)
    {
        if (rule.IsViolated()) throw new BusinessRuleViolationException(rule);
    }

    protected static async Task CheckRuleAsync(IBusinessRuleAsync rule)
    {
        if (await rule.IsViolatedAsync())
            throw new BusinessRuleAsyncViolationException(rule);
    }
}