using CompanyManager.Domain.Companies.Contracts;
using CompanyManager.Domain.Companies.Employees;
using CompanyManager.Domain.Shared.Contracts;

namespace CompanyManager.Domain.Companies.Rules;

public class EmployeeTitleMustBeUniqueWithinCompanyRule : IBusinessRuleAsync
{
    private readonly EmployeeTitle _employeeTitle;
    private readonly IEmployeeTitleWithinCompanyUniquenessChecker _employeeTitleWithinCompanyUniquenessChecker;

    public EmployeeTitleMustBeUniqueWithinCompanyRule(
        IEmployeeTitleWithinCompanyUniquenessChecker employeeTitleWithinCompanyUniquenessChecker, EmployeeTitle employeeTitle)
    {
        _employeeTitleWithinCompanyUniquenessChecker = employeeTitleWithinCompanyUniquenessChecker;
        _employeeTitle = employeeTitle;
    }

    public string Message => "Employee title must be unique within company.";

    public async Task<bool> IsViolatedAsync()
    {
        return await _employeeTitleWithinCompanyUniquenessChecker.IsUniqueAsync(_employeeTitle);
    }
}