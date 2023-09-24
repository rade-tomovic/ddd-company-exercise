using CompanyManager.Application.Companies.AddCompany;

namespace CompanyManager.Api.Controllers;

public record AddCompanyRequest(string Name, List<EmployeeToAdd> Employees);