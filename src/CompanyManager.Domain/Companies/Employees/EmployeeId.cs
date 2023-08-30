using CompanyManager.Domain.Shared.Contracts;

namespace CompanyManager.Domain.Companies.Employees;

public record EmployeeId(Guid Value)
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