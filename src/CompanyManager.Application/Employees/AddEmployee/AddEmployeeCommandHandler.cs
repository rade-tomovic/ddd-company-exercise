using CompanyManager.Application.Core;
using CompanyManager.Application.Core.Commands;
using CompanyManager.Application.Shared;
using CompanyManager.Domain.Companies;
using CompanyManager.Domain.Companies.Contracts;
using CompanyManager.Domain.Companies.Employees;
using MediatR;
using Serilog;

namespace CompanyManager.Application.Employees.AddEmployee;

public class AddEmployeeCommandHandler : ICommandHandler<AddEmployeeCommand, Guid>
{
    private readonly ICompanyRepository _companyRepository;
    private readonly IEmployeeEmailUniquenessChecker _employeeEmailUniquenessChecker;
    private readonly IEmployeeTitleWithinCompanyUniquenessChecker _employeeTitleWithinCompanyUniquenessChecker;
    private readonly ILogger _logger;
    private readonly ISystemLogPublisher _systemLogPublisher;
    private readonly IExecutionContextAccessor _contextAccessor;

    public AddEmployeeCommandHandler(IEmployeeEmailUniquenessChecker employeeEmailUniquenessChecker,
        IEmployeeTitleWithinCompanyUniquenessChecker employeeTitleWithinCompanyUniquenessChecker,
        ICompanyRepository companyRepository,
        ILogger logger, IMediator mediator, ISystemLogPublisher systemLogPublisher, IExecutionContextAccessor contextAccessor)
    {
        _employeeEmailUniquenessChecker = employeeEmailUniquenessChecker;
        _employeeTitleWithinCompanyUniquenessChecker = employeeTitleWithinCompanyUniquenessChecker;
        _companyRepository = companyRepository;
        _logger = logger;
        _systemLogPublisher = systemLogPublisher;
        _contextAccessor = contextAccessor;
    }

    public async Task<Guid> Handle(AddEmployeeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.Information(
                "Executing command {@Command}. Correlation ID: {CorrelationId}", request, _contextAccessor.CorrelationId);

            var employee = Employee.CreateNew(request.Email, request.Title);

            IEnumerable<Company> companies = await _companyRepository.GetByIdsAsync(request.CompanyIds);

            foreach (Company company in companies)
            {
                await company.AddEmployee(employee.Email, employee.Title, _employeeEmailUniquenessChecker,
                    _employeeTitleWithinCompanyUniquenessChecker);

                bool result = await _companyRepository.AddEmployeeToCompany(company);

                if (!result)
                    _logger.Error($"Failed to add employee {employee.Email} to company with id {company.Id}");

                _logger.Information($"Successfully added employee to company with id {company.Id}");

                await _systemLogPublisher.PublishDomainEvents(company, cancellationToken);
            }

            _logger.Information("Command processed successful, result {Result}. Correlation ID: {CorrelationId}", employee.Id, _contextAccessor.CorrelationId);

            return employee.Id;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Command processing failed. Correlation ID: {CorrelationId}", _contextAccessor.CorrelationId);
            throw;
        }
    }
}