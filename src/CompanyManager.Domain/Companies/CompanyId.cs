using CompanyManager.Domain.Shared.Contracts;

namespace CompanyManager.Domain.Companies;

public record CompanyId(Guid Value)
{
    private readonly Guid _value;

    public Guid Value
    {
        get => _value;
        init
        {
            Guard.AgainstEmptyGuid(value, nameof(CompanyId));
            _value = value;
        }
    }
}