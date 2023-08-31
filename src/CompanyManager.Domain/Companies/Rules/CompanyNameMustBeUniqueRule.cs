using CompanyManager.Domain.Companies.Contracts;
using CompanyManager.Domain.Shared.Contracts;

namespace CompanyManager.Domain.Companies.Rules;

public class CompanyNameMustBeUniqueRule : IBusinessRuleAsync
{
    private readonly ICompanyUniquenessChecker _companyUniquenessChecker;
    private readonly string _name;

    public CompanyNameMustBeUniqueRule(ICompanyUniquenessChecker companyUniquenessChecker, string name)
    {
        _companyUniquenessChecker = companyUniquenessChecker;
        _name = name;
    }

    public string Message => $"Company name must be unique. Company name: {_name}";

    public async Task<bool> IsViolatedAsync()
    {
        return await _companyUniquenessChecker.IsUniqueAsync(_name);
    }
}