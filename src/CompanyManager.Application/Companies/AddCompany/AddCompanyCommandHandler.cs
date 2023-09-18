using CompanyManager.Application.Company.AddCompany;
using CompanyManager.Application.Core.Commands;
using CompanyManager.Domain.Companies;
using CompanyManager.Domain.Companies.Contracts;
using CompanyManager.Domain.Companies.Employees;

namespace CompanyManager.Application.Companies.AddCompany;

public class AddCompanyCommandHandler : ICommandHandler<AddCompanyCommand, Guid>
{
    private readonly ICompanyRepository _companyRepository;
    private readonly ICompanyUniquenessChecker _companyUniquenssChecker;
    private readonly IEmployeeEmailUniquenessChecker _employeeEmailUniquenessChecker;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IEmployeeTitleWithinCompanyUniquenessChecker _employeeTitleWithinCompanyUniquenessChecker;

    public AddCompanyCommandHandler(ICompanyRepository companyRepository, ICompanyUniquenessChecker companyUniquenssChecker,
        IEmployeeEmailUniquenessChecker employeeEmailUniquenessChecker,
        IEmployeeTitleWithinCompanyUniquenessChecker employeeTitleWithinCompanyUniquenessChecker, IEmployeeRepository employeeRepository)
    {
        _companyRepository = companyRepository;
        _companyUniquenssChecker = companyUniquenssChecker;
        _employeeEmailUniquenessChecker = employeeEmailUniquenessChecker;
        _employeeTitleWithinCompanyUniquenessChecker = employeeTitleWithinCompanyUniquenessChecker;
        _employeeRepository = employeeRepository;
    }

    public async Task<Guid> Handle(AddCompanyCommand request, CancellationToken cancellationToken)
    {
        var company = await Domain.Companies.Company.CreateNew(request.CompanyName, _companyUniquenssChecker);
        List<Domain.Companies.Employees.Employee> allEmployees = PrepareEmployeeEntities(request);

        foreach (Domain.Companies.Employees.Employee employee in allEmployees)
            await company.AddEmployee(employee.Email, employee.Title, _employeeEmailUniquenessChecker,
                _employeeTitleWithinCompanyUniquenessChecker);

        CompanyId result = await _companyRepository.AddAsync(company);

        return result.Value;
    }

    private List<Domain.Companies.Employees.Employee> PrepareEmployeeEntities(AddCompanyCommand request)
    {
        List<EmployeeToAdd> newEmployees = request.Employees.Where(e => e.Id == null).ToList();
        List<EmployeeToAdd> existingEmployees = request.Employees.Where(e => e.Id != null).ToList();

        List<Domain.Companies.Employees.Employee> existingEmployeeEntities =
            _employeeRepository.GetByIdsAsync(existingEmployees.Select(e => e.Id!.Value).ToArray()).Result;
        List<Domain.Companies.Employees.Employee> newEmployeeEntities = newEmployees
            .Select(x => Domain.Companies.Employees.Employee.CreateNew(x.Email!, x.Title!.Value)).ToList();

        List<Domain.Companies.Employees.Employee> allEmployees = new();
        allEmployees.AddRange(existingEmployeeEntities);
        allEmployees.AddRange(newEmployeeEntities);

        return allEmployees;
    }
}