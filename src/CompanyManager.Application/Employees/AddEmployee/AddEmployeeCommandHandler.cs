using CompanyManager.Application.Core.Commands;
using CompanyManager.Application.Shared;
using CompanyManager.Domain.Companies;
using CompanyManager.Domain.Companies.Contracts;
using CompanyManager.Domain.Companies.Employees;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CompanyManager.Application.Employees.AddEmployee;

public class AddEmployeeCommandHandler : ICommandHandler<AddEmployeeCommand, Guid>
{
    private readonly ICompanyRepository _companyRepository;
    private readonly IEmployeeEmailUniquenessChecker _employeeEmailUniquenessChecker;
    private readonly IEmployeeTitleWithinCompanyUniquenessChecker _employeeTitleWithinCompanyUniquenessChecker;
    private readonly ILogger<AddEmployeeCommandHandler> _logger;
    private readonly ISystemLogPublisher _systemLogPublisher;

    public AddEmployeeCommandHandler(IEmployeeEmailUniquenessChecker employeeEmailUniquenessChecker,
        IEmployeeTitleWithinCompanyUniquenessChecker employeeTitleWithinCompanyUniquenessChecker,
        ICompanyRepository companyRepository,
        ILogger<AddEmployeeCommandHandler> logger, IMediator mediator, ISystemLogPublisher systemLogPublisher)
    {
        _employeeEmailUniquenessChecker = employeeEmailUniquenessChecker;
        _employeeTitleWithinCompanyUniquenessChecker = employeeTitleWithinCompanyUniquenessChecker;
        _companyRepository = companyRepository;
        _logger = logger;
        _systemLogPublisher = systemLogPublisher;
    }

    public async Task<Guid> Handle(AddEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = Employee.CreateNew(request.Email, request.Title);

        var companies = await _companyRepository.GetByIdsAsync(request.CompanyIds);

        await foreach (var company in companies.WithCancellation(cancellationToken))
        {
            await company.AddEmployee(employee.Email, employee.Title, _employeeEmailUniquenessChecker,
                _employeeTitleWithinCompanyUniquenessChecker);

            var result = await _companyRepository.UpdateAsync(company);

            if (!result)
                _logger.LogError($"Failed to add employee {employee.Email} to company with id {company.Id}");

            _logger.LogInformation($"Successfully added employee to company with id {company.Id}");

            await _systemLogPublisher.PublishDomainEvents(company, cancellationToken);
        }

        return employee.Id;
    }
}