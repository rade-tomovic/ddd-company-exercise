using CompanyManager.Application.Core.Commands;
using CompanyManager.Domain.Companies;
using CompanyManager.Domain.Companies.Contracts;
using CompanyManager.Domain.Companies.Employees;
using Microsoft.Extensions.Logging;

namespace CompanyManager.Application.Companies.AddCompany;

public class AddCompanyCommandHandler : ICommandHandler<AddCompanyCommand, Guid>
{
    private readonly ICompanyRepository _companyRepository;
    private readonly ICompanyUniquenessChecker _companyUniquenssChecker;
    private readonly IEmployeeEmailUniquenessChecker _employeeEmailUniquenessChecker;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IEmployeeTitleWithinCompanyUniquenessChecker _employeeTitleWithinCompanyUniquenessChecker;
    private readonly ILogger<AddCompanyCommandHandler> _logger;

    public AddCompanyCommandHandler(ICompanyRepository companyRepository, ICompanyUniquenessChecker companyUniquenssChecker,
        IEmployeeEmailUniquenessChecker employeeEmailUniquenessChecker,
        IEmployeeTitleWithinCompanyUniquenessChecker employeeTitleWithinCompanyUniquenessChecker, IEmployeeRepository employeeRepository, ILogger<AddCompanyCommandHandler> logger)
    {
        _companyRepository = companyRepository;
        _companyUniquenssChecker = companyUniquenssChecker;
        _employeeEmailUniquenessChecker = employeeEmailUniquenessChecker;
        _employeeTitleWithinCompanyUniquenessChecker = employeeTitleWithinCompanyUniquenessChecker;
        _employeeRepository = employeeRepository;
        _logger = logger;
    }

    public async Task<Guid> Handle(AddCompanyCommand request, CancellationToken cancellationToken)
    {
        var company = await Company.CreateNew(request.CompanyName, _companyUniquenssChecker);
        List<Employee> allEmployees = PrepareEmployeeEntities(request);

        foreach (Employee employee in allEmployees)
            await company.AddEmployee(employee.Email, employee.Title, _employeeEmailUniquenessChecker,
                _employeeTitleWithinCompanyUniquenessChecker);

        CompanyId result = await _companyRepository.AddAsync(company);

        return result.Value;
    }

    private List<Employee> PrepareEmployeeEntities(AddCompanyCommand request)
    {
        List<EmployeeToAdd> newEmployees = request.Employees.Where(e => e.Id == null).ToList();
        List<EmployeeToAdd> existingEmployees = request.Employees.Where(e => e.Id != null).ToList();

        List<Employee> existingEmployeeEntities =
            _employeeRepository.GetByIdsAsync(existingEmployees.Select(e => e.Id!.Value).ToArray()).Result;
        List<Employee> newEmployeeEntities = newEmployees
            .Select(x => Employee.CreateNew(x.Email!, x.Title!.Value)).ToList();

        List<Employee> allEmployees = new();
        allEmployees.AddRange(existingEmployeeEntities);
        allEmployees.AddRange(newEmployeeEntities);

        return allEmployees;
    }
}