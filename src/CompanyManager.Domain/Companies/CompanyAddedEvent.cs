using CompanyManager.Domain.Shared.Core;

namespace CompanyManager.Domain.Companies;

public sealed class CompanyAddedEvent : DomainEventBase
{
    public CompanyAddedEvent(Company company)
    {
        Comment = $"Company {company.Name} added.";
        EventAction = "Added";
        ResourceType = nameof(Company);
        Entity = company;
    }
}