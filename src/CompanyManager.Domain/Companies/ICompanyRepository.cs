using CompanyManager.Domain.Companies.Employees;

namespace CompanyManager.Domain.Companies;

public interface ICompanyRepository
{
    Task<CompanyId> AddAsync(Company company);
    Task<IEnumerable<Company>> GetByIdsAsync(List<Guid> companyIds);
    Task<bool> UpdateAsync(Company company);
    Task<bool> IsNameUniqueAsync(string name);
    Task<bool> IsEmployeeEmailUniqueAsync(string email, Guid companyId);
    Task<bool> IsEmployeeTitleWithinCompanyUniqueAsync(EmployeeTitle title, Guid companyId);
}