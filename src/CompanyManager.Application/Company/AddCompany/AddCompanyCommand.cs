using CompanyManager.Application.Core.Commands;

namespace CompanyManager.Application.Company.AddCompany;

public class AddCompanyCommand : CommandBase<Guid>
{
    public AddCompanyCommand(string companyName, List<EmployeeToAdd> employees)
    {
        CompanyName = companyName;
        Employees = employees;
    }

    public string CompanyName { get; }
    public List<EmployeeToAdd> Employees { get; }
}