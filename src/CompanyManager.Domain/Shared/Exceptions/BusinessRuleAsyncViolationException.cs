using CompanyManager.Domain.Shared.Contracts;

namespace CompanyManager.Domain.Shared.Exceptions;

public class BusinessRuleAsyncViolationException : Exception
{
    public BusinessRuleAsyncViolationException(IBusinessRuleAsync brokenRule) : base(brokenRule.Message)
    {
        BrokenRule = brokenRule;
        Details = brokenRule.Message;
    }

    public IBusinessRuleAsync BrokenRule { get; }

    public string Details { get; }

    public override string ToString()
    {
        return $"{BrokenRule.GetType().FullName}: {BrokenRule.Message}";
    }
}