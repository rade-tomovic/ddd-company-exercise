using CompanyManager.Domain.Companies.Employees;

namespace CompanyManager.Domain.Companies.Contracts;

public interface IEmployeeTitleWithinCompanyUniquenessChecker
{
    Task<bool> IsUniqueAsync(EmployeeTitle title, Guid companyId);
}