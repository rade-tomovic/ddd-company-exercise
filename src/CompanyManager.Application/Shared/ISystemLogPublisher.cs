using CompanyManager.Domain.Companies;

namespace CompanyManager.Application.Shared;

public interface ISystemLogPublisher
{
    Task PublishDomainEvents(Company company, CancellationToken cancellationToken);
}