using CompanyManager.Domain.Shared.Contracts;

namespace CompanyManager.Domain.Companies.Employees;

public record EmployeeId
{
    public EmployeeId(Guid value)
    {
        Guard.AgainstEmptyGuid(value, nameof(EmployeeId));
        Value = value;
    }

    public Guid Value { get; }
}