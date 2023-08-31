using CompanyManager.Domain.Shared.Core;

namespace CompanyManager.Domain.Companies;

public class CompanyAddedEvent : DomainEventBase
{
    public CompanyAddedEvent(Company company)
    {
        Company = company;
    }

    public Company Company { get; }
}