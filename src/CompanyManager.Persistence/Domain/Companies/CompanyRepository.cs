using CompanyManager.Domain.Companies;
using CompanyManager.Domain.Companies.Employees;

namespace CompanyManager.Persistence.Domain.Companies;

public class CompanyRepository : ICompanyRepository
{
    public async Task<CompanyId> AddAsync(Company company)
    {
        throw new NotImplementedException();
    }

    public async Task<IAsyncEnumerable<Company>> GetByIdsAsync(List<Guid> companyIds)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> UpdateAsync(Company company)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> IsNameUniqueAsync(string name)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> IsEmployeeEmailUniqueAsync(string email, Guid companyId)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> IsEmployeeTitleWithinCompanyUniqueAsync(EmployeeTitle title, Guid companyId)
    {
        throw new NotImplementedException();
    }
}