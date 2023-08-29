using CompanyManager.Domain.Shared;

namespace CompanyManager.Domain.Companies.Employees;

public record EmployeeId(Guid Value)
{
    private readonly Guid _value;

    public Guid Value
    {
        get => _value;
        init
        {
            Guard.AgainstEmptyGuid(value, nameof(EmployeeId));
            _value = value;
        }
    }
}