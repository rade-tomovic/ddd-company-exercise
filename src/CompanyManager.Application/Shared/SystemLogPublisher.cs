using CompanyManager.Application.SystemLogs;
using CompanyManager.Domain.Companies;
using CompanyManager.Domain.Shared.Contracts;
using CompanyManager.Domain.Shared.Core;
using CompanyManager.Domain.SystemLogs;
using MediatR;
using Serilog;

namespace CompanyManager.Application.Shared;

public class SystemLogPublisher : ISystemLogPublisher
{
    private readonly ILogger _logger;
    private readonly IMediator _mediator;

    public SystemLogPublisher(IMediator mediator, ILogger logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task PublishDomainEvents(Company company, CancellationToken cancellationToken)
    {
        if (company.DomainEvents == null)
        {
            _logger.Error($"Domain events are not populated for company {company.Id}");
            throw new ArgumentOutOfRangeException($"Domain events are not populated for company {company.Id}");
        }

        foreach (var domainEvent in company.DomainEvents)
        {
            var systemLog = SystemLog<IDomainEvent<Entity>, Entity>.CreateNew(domainEvent);
            await _mediator.Publish(new SystemLogNotification(systemLog), cancellationToken);
        }
    }
}