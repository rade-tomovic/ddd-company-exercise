using CompanyManager.Domain.Companies.Contracts;
using CompanyManager.Domain.Shared.Contracts;

namespace CompanyManager.Domain.Companies.Rules;

public class EmployeeEmailMustBeUniqueRule : IBusinessRuleAsync
{
    private readonly IEmployeeEmailUniquenessChecker _emailUniquenessChecker;
    private readonly string _email;

    public EmployeeEmailMustBeUniqueRule(IEmployeeEmailUniquenessChecker emailUniquenessChecker, string email)
    {
        _emailUniquenessChecker = emailUniquenessChecker;
        _email = email;
    }

    public string Message => $"Employee email must be unique. Email: {_email}";
    public async Task<bool> IsViolatedAsync() => await _emailUniquenessChecker.IsUniqueAsync(_email);
}