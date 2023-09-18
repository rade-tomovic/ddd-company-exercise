namespace CompanyManager.Domain.Companies;

public interface ICompanyRepository
{
    Task<CompanyId> AddAsync(Company company);
    Task<IAsyncEnumerable<Company>> GetByIdsAsync(List<Guid> companyIds);
    Task UpdateAsync(Company company);
}