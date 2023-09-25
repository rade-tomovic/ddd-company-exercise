using CompanyManager.Application.Core;
using CompanyManager.Application.Core.Commands;
using CompanyManager.Application.Shared;
using CompanyManager.Domain.Companies;
using CompanyManager.Domain.Companies.Contracts;
using CompanyManager.Domain.Companies.Employees;
using Serilog;

namespace CompanyManager.Application.Companies.AddCompany;

public class AddCompanyCommandHandler : ICommandHandler<AddCompanyCommand, Guid>
{
    private readonly ICompanyRepository _companyRepository;
    private readonly ICompanyUniquenessChecker _companyUniquenessChecker;
    private readonly IEmployeeEmailUniquenessChecker _employeeEmailUniquenessChecker;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IEmployeeTitleWithinCompanyUniquenessChecker _employeeTitleWithinCompanyUniquenessChecker;
    private readonly ILogger _logger;
    private readonly ISystemLogPublisher _systemLogPublisher;
    private readonly IExecutionContextAccessor _contextAccessor;

    public AddCompanyCommandHandler(ICompanyRepository companyRepository,
        ICompanyUniquenessChecker companyUniquenessChecker,
        IEmployeeEmailUniquenessChecker employeeEmailUniquenessChecker,
        IEmployeeTitleWithinCompanyUniquenessChecker employeeTitleWithinCompanyUniquenessChecker,
        IEmployeeRepository employeeRepository, ILogger logger,
        ISystemLogPublisher systemLogPublisher, IExecutionContextAccessor contextAccessor)
    {
        _companyRepository = companyRepository;
        _companyUniquenessChecker = companyUniquenessChecker;
        _employeeEmailUniquenessChecker = employeeEmailUniquenessChecker;
        _employeeTitleWithinCompanyUniquenessChecker = employeeTitleWithinCompanyUniquenessChecker;
        _employeeRepository = employeeRepository;
        _logger = logger;
        _systemLogPublisher = systemLogPublisher;
        _contextAccessor = contextAccessor;
    }

    public async Task<Guid> Handle(AddCompanyCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.Information(
                "Executing command {@Command}. Correlation ID: {CorrelationId}", request, _contextAccessor.CorrelationId);

            var company = await Company.CreateNew(request.CompanyName, _companyUniquenessChecker);
            var allEmployees = await PrepareEmployeeEntities(request);

            foreach (var employee in allEmployees)
                await company.AddEmployee(employee.Email, employee.Title, _employeeEmailUniquenessChecker,
                    _employeeTitleWithinCompanyUniquenessChecker);

            var result = await _companyRepository.AddAsync(company);
            await _systemLogPublisher.PublishDomainEvents(company, cancellationToken);

            _logger.Information("Command processed successful, result {Result}. Correlation ID: {CorrelationId}", result, _contextAccessor.CorrelationId);

            return result.Value;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Command processing failed. Correlation ID: {CorrelationId}", _contextAccessor.CorrelationId);
            throw;
        }
    }

    private async Task<List<Employee>> PrepareEmployeeEntities(AddCompanyCommand request)
    {
        var newEmployees = request.Employees.Where(e => e.Id == null).ToList();
        var existingEmployees = request.Employees.Where(e => e.Id != null).ToList();
        List<Employee> allEmployees = new();

        if (existingEmployees.Any())
        {
            var existingEmployeeEntities = await _employeeRepository.GetByIdsAsync(existingEmployees.Select(e => e.Id!.Value).ToArray());
            allEmployees.AddRange(existingEmployeeEntities);
        }
        
        var newEmployeeEntities = newEmployees
            .Select(x => Employee.CreateNew(x.Email!, x.Title!.Value)).ToList();

        allEmployees.AddRange(newEmployeeEntities);

        return allEmployees;
    }
}