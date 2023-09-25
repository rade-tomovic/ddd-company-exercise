using CompanyManager.Application.Core.Commands;
using CompanyManager.Domain.Companies.Employees;

namespace CompanyManager.Application.Employees.AddEmployee;

public class AddEmployeeCommand : ICommand<Guid>
{
    public AddEmployeeCommand(List<Guid> companyIds, EmployeeTitle title, string email)
    {
        CompanyIds = companyIds;
        Title = title;
        Email = email;
    }

    public string Email { get; set; }
    public EmployeeTitle Title { get; set; }
    public List<Guid> CompanyIds { get; set; }
    public Guid Id { get; }
}