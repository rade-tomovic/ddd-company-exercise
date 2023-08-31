using CompanyManager.Domain.Companies.Contracts;
using CompanyManager.Domain.Shared.Contracts;

namespace CompanyManager.Domain.Companies.Rules;

public class EmployeeEmailMustBeUniqueRule : IBusinessRuleAsync
{
    private readonly string _email;
    private readonly IEmployeeEmailUniquenessChecker _emailUniquenessChecker;

    public EmployeeEmailMustBeUniqueRule(IEmployeeEmailUniquenessChecker emailUniquenessChecker, string email)
    {
        _emailUniquenessChecker = emailUniquenessChecker;
        _email = email;
    }

    public string Message => $"Employee email must be unique. Email: {_email}";

    public async Task<bool> IsViolatedAsync()
    {
        return !await _emailUniquenessChecker.IsUniqueAsync(_email);
    }
}