using CompanyManager.Domain.Shared.Contracts;

namespace CompanyManager.Domain.Companies;

public record CompanyId
{
    public CompanyId(Guid value)
    {
        Guard.AgainstEmptyGuid(value, nameof(CompanyId));
        Value = value;
    }

    public Guid Value { get; }
}