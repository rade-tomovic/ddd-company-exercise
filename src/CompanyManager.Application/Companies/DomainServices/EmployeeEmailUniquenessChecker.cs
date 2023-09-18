using CompanyManager.Domain.Companies;
using CompanyManager.Domain.Companies.Contracts;

namespace CompanyManager.Application.Companies.DomainServices;

public class EmployeeEmailUniquenessChecker : IEmployeeEmailUniquenessChecker
{
    private readonly ICompanyRepository _companyRepository;

    public EmployeeEmailUniquenessChecker(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository;
    }

    public async Task<bool> IsUniqueAsync(string email, Guid companyId)
    {
        return await _companyRepository.IsEmployeeEmailUniqueAsync(email, companyId);
    }
}