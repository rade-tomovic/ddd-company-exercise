namespace CompanyManager.Domain.Shared.Contracts;

public interface IBusinessRule
{
    string Message { get; }
    bool IsViolated();
}