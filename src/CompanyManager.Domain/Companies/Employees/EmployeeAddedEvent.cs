using CompanyManager.Domain.Shared.Core;

namespace CompanyManager.Domain.Companies.Employees;

public class EmployeeAddedEvent : DomainEventBase
{
    public EmployeeAddedEvent(Employee employee, CompanyId companyId)
    {
        Employee = employee;
        CompanyId = companyId;
    }

    public Employee Employee { get; }
    public CompanyId CompanyId { get; }
}