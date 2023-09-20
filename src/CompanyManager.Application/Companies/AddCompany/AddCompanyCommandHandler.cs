using CompanyManager.Application.Core.Commands;
using CompanyManager.Application.Shared;
using CompanyManager.Domain.Companies;
using CompanyManager.Domain.Companies.Contracts;
using CompanyManager.Domain.Companies.Employees;
using Microsoft.Extensions.Logging;

namespace CompanyManager.Application.Companies.AddCompany;

public class AddCompanyCommandHandler : ICommandHandler<AddCompanyCommand, Guid>
{
    private readonly ICompanyRepository _companyRepository;
    private readonly ICompanyUniquenessChecker _companyUniquenessChecker;
    private readonly IEmployeeEmailUniquenessChecker _employeeEmailUniquenessChecker;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IEmployeeTitleWithinCompanyUniquenessChecker _employeeTitleWithinCompanyUniquenessChecker;
    private readonly ILogger<AddCompanyCommandHandler> _logger;
    private readonly ISystemLogPublisher _systemLogPublisher;

    public AddCompanyCommandHandler(ICompanyRepository companyRepository,
        ICompanyUniquenessChecker companyUniquenessChecker,
        IEmployeeEmailUniquenessChecker employeeEmailUniquenessChecker,
        IEmployeeTitleWithinCompanyUniquenessChecker employeeTitleWithinCompanyUniquenessChecker,
        IEmployeeRepository employeeRepository, ILogger<AddCompanyCommandHandler> logger,
        ISystemLogPublisher systemLogPublisher)
    {
        _companyRepository = companyRepository;
        _companyUniquenessChecker = companyUniquenessChecker;
        _employeeEmailUniquenessChecker = employeeEmailUniquenessChecker;
        _employeeTitleWithinCompanyUniquenessChecker = employeeTitleWithinCompanyUniquenessChecker;
        _employeeRepository = employeeRepository;
        _logger = logger;
        _systemLogPublisher = systemLogPublisher;
    }

    public async Task<Guid> Handle(AddCompanyCommand request, CancellationToken cancellationToken)
    {
        var company = await Company.CreateNew(request.CompanyName, _companyUniquenessChecker);
        var allEmployees = PrepareEmployeeEntities(request);

        foreach (var employee in allEmployees)
            await company.AddEmployee(employee.Email, employee.Title, _employeeEmailUniquenessChecker,
                _employeeTitleWithinCompanyUniquenessChecker);

        var result = await _companyRepository.AddAsync(company);
        await _systemLogPublisher.PublishDomainEvents(company, cancellationToken);

        return result.Value;
    }

    private List<Employee> PrepareEmployeeEntities(AddCompanyCommand request)
    {
        var newEmployees = request.Employees.Where(e => e.Id == null).ToList();
        var existingEmployees = request.Employees.Where(e => e.Id != null).ToList();

        var existingEmployeeEntities =
            _employeeRepository.GetByIdsAsync(existingEmployees.Select(e => e.Id!.Value).ToArray()).Result;
        var newEmployeeEntities = newEmployees
            .Select(x => Employee.CreateNew(x.Email!, x.Title!.Value)).ToList();

        List<Employee> allEmployees = new();
        allEmployees.AddRange(existingEmployeeEntities);
        allEmployees.AddRange(newEmployeeEntities);

        return allEmployees;
    }
}