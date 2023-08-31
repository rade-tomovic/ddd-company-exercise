using CompanyManager.Domain.Companies.Contracts;
using CompanyManager.Domain.Companies.Employees;
using CompanyManager.Domain.Companies.Rules;
using CompanyManager.Domain.Shared.Contracts;
using CompanyManager.Domain.Shared.Core;

namespace CompanyManager.Domain.Companies;

public class Company : Entity, IAuditable
{
    private readonly List<Employee> _employees;
    private readonly CompanyId _id;
    private readonly string _name;

    private Company()
    {
        _employees = new List<Employee>();
    }

    private Company(string name, List<Employee> employees)
    {
        _name = name;
        _employees = employees;
        _id = new CompanyId(Guid.NewGuid());
        CreatedAt = TimeProvider.Now;

        AddDomainEvent(new CompanyAddedEvent(this));
    }

    public DateTime CreatedAt { get; }

    public static async Task<Company> CreateNew(string name, List<Employee> employees, ICompanyUniquenessChecker companyUniquenessChecker,
        IEmployeeEmailUniquenessChecker emailUniquenessChecker,
        IEmployeeTitleWithinCompanyUniquenessChecker titleWithinCompanyUniquenessChecker)
    {
        await CheckRuleAsync(new CompanyNameMustBeUniqueRule(companyUniquenessChecker, name));

        return new Company(name, employees);
    }

    public async Task<EmployeeId> AddEmployee(string email, EmployeeTitle title, IEmployeeEmailUniquenessChecker emailUniquenessChecker,
        IEmployeeTitleWithinCompanyUniquenessChecker titleWithinCompanyUniquenessChecker)
    {
        await CheckRuleAsync(new EmployeeEmailMustBeUniqueRule(emailUniquenessChecker, email));
        await CheckRuleAsync(new EmployeeTitleMustBeUniqueWithinCompanyRule(titleWithinCompanyUniquenessChecker, title));

        var employee = Employee.CreateNew(email, title);

        _employees.Add(employee);

        AddDomainEvent(new EmployeeAddedEvent(employee, _id));

        return new EmployeeId(employee.Id);
    }
}