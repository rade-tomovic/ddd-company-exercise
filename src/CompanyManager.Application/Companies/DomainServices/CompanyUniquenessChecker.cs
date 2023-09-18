using CompanyManager.Domain.Companies;
using CompanyManager.Domain.Companies.Contracts;

namespace CompanyManager.Application.Companies.DomainServices;

public class CompanyUniquenessChecker : ICompanyUniquenessChecker
{
    private readonly ICompanyRepository _companyRepository;

    public CompanyUniquenessChecker(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository;
    }

    public async Task<bool> IsUniqueAsync(string name)
    {
        return await _companyRepository.IsNameUniqueAsync(name);
    }
}