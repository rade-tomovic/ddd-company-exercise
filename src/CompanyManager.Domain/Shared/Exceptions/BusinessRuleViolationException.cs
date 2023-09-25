using CompanyManager.Domain.Shared.Contracts;

namespace CompanyManager.Domain.Shared.Exceptions;

public class BusinessRuleViolationException : Exception
{
    public BusinessRuleViolationException(IBusinessRule brokenRule) : base(brokenRule.Message)
    {
        BrokenRule = brokenRule;
        Details = brokenRule.Message;
    }

    public IBusinessRule BrokenRule { get; }

    public string Details { get; }

    public override string ToString()
    {
        return $"{BrokenRule.GetType().FullName}: {BrokenRule.Message}";
    }
}