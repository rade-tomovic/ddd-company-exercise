using CompanyManager.Domain.Companies;
using CompanyManager.Domain.Companies.Contracts;
using CompanyManager.Domain.Companies.Employees;

namespace CompanyManager.Application.Companies.DomainServices;

public class EmployeeTitleWithinCompanyUniquenessChecker : IEmployeeTitleWithinCompanyUniquenessChecker
{
    private readonly ICompanyRepository _companyRepository;

    public EmployeeTitleWithinCompanyUniquenessChecker(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository;
    }

    public async Task<bool> IsUniqueAsync(EmployeeTitle title, Guid companyId)
    {
        return await _companyRepository.IsEmployeeTitleWithinCompanyUniqueAsync(title, companyId);
    }
}