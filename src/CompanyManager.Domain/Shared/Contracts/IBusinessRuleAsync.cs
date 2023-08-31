namespace CompanyManager.Domain.Shared.Contracts;

public interface IBusinessRuleAsync
{
    string Message { get; }
    Task<bool> IsViolatedAsync();
}