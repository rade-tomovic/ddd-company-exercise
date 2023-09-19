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

    private Company(string name)
    {
        Name = name;
        _employees = new List<Employee>();
        _id = new CompanyId(Guid.NewGuid());
        CreatedAt = TimeProvider.Now;

        AddDomainEvent(new CompanyAddedEvent(this));
    }

    public Guid Id => _id.Value;
    public string Name { get; }
    public IReadOnlyCollection<Employee> Employees => _employees.AsReadOnly();
    public DateTime CreatedAt { get; }

    public static async Task<Company> CreateNew(string name, ICompanyUniquenessChecker companyUniquenessChecker)
    {
        await CheckRuleAsync(new CompanyNameMustBeUniqueRule(companyUniquenessChecker, name));

        return new Company(name);
    }

    public async Task<EmployeeId> AddEmployee(string email, EmployeeTitle title, IEmployeeEmailUniquenessChecker emailUniquenessChecker,
        IEmployeeTitleWithinCompanyUniquenessChecker titleWithinCompanyUniquenessChecker)
    {
        await CheckRuleAsync(new EmployeeEmailMustBeUniqueRule(emailUniquenessChecker, email, _id));
        await CheckRuleAsync(new EmployeeTitleMustBeUniqueWithinCompanyRule(titleWithinCompanyUniquenessChecker, title, _id));

        var employee = Employee.CreateNew(email, title);

        _employees.Add(employee);

        AddDomainEvent(new EmployeeAddedEvent(employee, _id));

        return new EmployeeId(employee.Id);
    }
}