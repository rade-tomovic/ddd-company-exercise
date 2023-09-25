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
            List<Employee> allEmployees = await PrepareEmployeeEntities(request);

            foreach (Employee employee in allEmployees)
                await company.AddEmployee(employee.Email, employee.Title, _employeeEmailUniquenessChecker,
                    _employeeTitleWithinCompanyUniquenessChecker);

            CompanyId result = await _companyRepository.AddAsync(company);
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
        List<EmployeeToAdd> newEmployees = request.Employees.Where(e => e.Id == null).ToList();
        List<EmployeeToAdd> existingEmployees = request.Employees.Where(e => e.Id != null).ToList();
        List<Employee> allEmployees = new();

        if (existingEmployees.Any())
        {
            List<Employee> existingEmployeeEntities = await _employeeRepository.GetByIdsAsync(existingEmployees.Select(e => e.Id!.Value).ToArray());
            allEmployees.AddRange(existingEmployeeEntities);
        }
        
        List<Employee> newEmployeeEntities = newEmployees
            .Select(x => Employee.CreateNew(x.Email!, x.Title!.Value)).ToList();

        allEmployees.AddRange(newEmployeeEntities);

        ValidateDuplicateEmails(allEmployees);
        ValidateDuplicateTitles(allEmployees);

        return allEmployees;
    }

    private static void ValidateDuplicateEmails(List<Employee> employees)
    {
        var duplicateEmails = employees
            .GroupBy(e => e.Email)
            .Where(g => g.Count() > 1)
            .Select(g => new { Email = g.Key, Count = g.Count() })
            .ToList();

        if (!duplicateEmails.Any())
            return;

        IEnumerable<string> errorMessages = duplicateEmails.Select(d => $"Email: {d.Email} (Count: {d.Count})");
        throw new InvalidOperationException($"Duplicate emails found: {string.Join(", ", errorMessages)}");
    }

    private static void ValidateDuplicateTitles(List<Employee> employees)
    {
        var duplicateTitles = employees
            .GroupBy(e => e.Title)
            .Where(g => g.Count() > 1)
            .Select(g => new { Title = g.Key, Emails = g.Select(e => e.Email).ToList() })
            .ToList();

        if (!duplicateTitles.Any()) 
            return;

        IEnumerable<string> errorMessages = duplicateTitles.Select(d => $"Title: {d.Title} (Emails: {string.Join(", ", d.Emails)})");
        throw new InvalidOperationException($"Duplicate titles found: {string.Join("; ", errorMessages)}");
    }
}