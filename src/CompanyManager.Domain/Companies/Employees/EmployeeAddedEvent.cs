using CompanyManager.Domain.Shared.Core;

namespace CompanyManager.Domain.Companies.Employees;

public sealed class EmployeeAddedEvent : DomainEventBase
{
    public EmployeeAddedEvent(Employee employee, CompanyId companyId)
    {
        Entity = employee;
        Comment = $"Employee {employee.Email} added to company {companyId.Value}";
        EventAction = "Added";
        ResourceType = nameof(Employee);
    }
}