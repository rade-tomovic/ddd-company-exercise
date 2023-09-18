using CompanyManager.Domain.Companies.Employees;

namespace CompanyManager.Application.Employee.AddEmployee;

public class AddEmployeeCommand
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
}