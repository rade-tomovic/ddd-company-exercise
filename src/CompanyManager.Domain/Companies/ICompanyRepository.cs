namespace CompanyManager.Domain.Companies;

public interface ICompanyRepository
{
    Task<CompanyId> AddAsync(Company company);
}