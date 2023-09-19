using CompanyManager.Application.Core.Commands;
using CompanyManager.Application.SystemLogs;
using CompanyManager.Domain.Companies;
using CompanyManager.Domain.Companies.Contracts;
using CompanyManager.Domain.Companies.Employees;
using CompanyManager.Domain.Shared.Contracts;
using CompanyManager.Domain.Shared.Core;
using CompanyManager.Domain.SystemLogs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CompanyManager.Application.Employees.AddEmployee;

public class AddEmployeeCommandHandler : ICommandHandler<AddEmployeeCommand, Guid>
{
    private readonly ICompanyRepository _companyRepository;
    private readonly IEmployeeEmailUniquenessChecker _employeeEmailUniquenessChecker;
    private readonly IEmployeeTitleWithinCompanyUniquenessChecker _employeeTitleWithinCompanyUniquenessChecker;
    private readonly ILogger<AddEmployeeCommandHandler> _logger;
    private readonly IMediator _mediator;

    public AddEmployeeCommandHandler(IEmployeeEmailUniquenessChecker employeeEmailUniquenessChecker,
        IEmployeeTitleWithinCompanyUniquenessChecker employeeTitleWithinCompanyUniquenessChecker, ICompanyRepository companyRepository,
        ILogger<AddEmployeeCommandHandler> logger, IMediator mediator)
    {
        _employeeEmailUniquenessChecker = employeeEmailUniquenessChecker;
        _employeeTitleWithinCompanyUniquenessChecker = employeeTitleWithinCompanyUniquenessChecker;
        _companyRepository = companyRepository;
        _logger = logger;
        _mediator = mediator;
    }

    public async Task<Guid> Handle(AddEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = Employee.CreateNew(request.Email, request.Title);

        IAsyncEnumerable<Company> companies = await _companyRepository.GetByIdsAsync(request.CompanyIds);

        await foreach (Company company in companies.WithCancellation(cancellationToken))
        {
            await company.AddEmployee(employee.Email, employee.Title, _employeeEmailUniquenessChecker,
                _employeeTitleWithinCompanyUniquenessChecker);

            bool result = await _companyRepository.UpdateAsync(company);

            if (!result)
                _logger.LogError($"Failed to add employee {employee.Email} to company with id {company.Id}");

            _logger.LogInformation($"Successfully added employee to company with id {company.Id}");

            if (company.DomainEvents == null)
            {
                _logger.LogError($"Domain events are not populated for company {company.Id}");
                throw new ArgumentOutOfRangeException($"Domain events are not populated for company {company.Id}");
            }

            foreach (IDomainEvent<Entity> domainEvent in company.DomainEvents)
            {
                SystemLog<IDomainEvent<Entity>, Entity> systemLog = SystemLog<IDomainEvent<Entity>, Entity>.CreateNew(domainEvent);

                await _mediator.Publish(new SystemLogNotification(systemLog), cancellationToken);
            }
        }

        return employee.Id;
    }
}