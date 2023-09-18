using CompanyManager.Application.Company.AddCompany;
using CompanyManager.Application.Core.Commands;

namespace CompanyManager.Application.Companies.AddCompany;

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