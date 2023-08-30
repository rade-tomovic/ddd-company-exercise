using CompanyManager.Domain.Shared.Contracts;
using CompanyManager.Domain.Shared.Core;
using CompanyManager.Domain.Shared.ValueObjects;

namespace CompanyManager.Domain.Companies.Employees;

public class Employee : Entity, IAuditable
{
    private readonly Email _email;
    private readonly EmployeeId _id;

    private Employee(string email, EmployeeTitle title)
    {
        _email = new Email(email);
        Title = title;
        _id = new EmployeeId(Guid.NewGuid());
        CreatedAt = TimeProvider.Now;
    }

    public string Email => _email.Value;
    public EmployeeTitle Title { get; }
    public Guid Id => _id.Value;
    public DateTime CreatedAt { get; }

    internal static Employee CreateNew(string email, EmployeeTitle title)
    {
        return new Employee(email, title);
    }
}