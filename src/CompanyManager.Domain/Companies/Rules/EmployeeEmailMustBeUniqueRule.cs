using CompanyManager.Domain.Companies.Contracts;
using CompanyManager.Domain.Shared.Contracts;

namespace CompanyManager.Domain.Companies.Rules;

public class EmployeeEmailMustBeUniqueRule : IBusinessRuleAsync
{
    private readonly string _email;
    private readonly CompanyId _companyId;
    private readonly IEmployeeEmailUniquenessChecker _emailUniquenessChecker;

    public EmployeeEmailMustBeUniqueRule(IEmployeeEmailUniquenessChecker emailUniquenessChecker, string email, CompanyId companyId)
    {
        _emailUniquenessChecker = emailUniquenessChecker;
        _email = email;
        _companyId = companyId;
    }

    public string Message => $"Employee email must be unique. Email: {_email}";

    public async Task<bool> IsViolatedAsync()
    {
        return !await _emailUniquenessChecker.IsUniqueAsync(_email, _companyId.Value);
    }
}