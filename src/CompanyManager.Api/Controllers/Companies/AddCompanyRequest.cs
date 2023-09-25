using CompanyManager.Application.Companies.AddCompany;

namespace CompanyManager.Api.Controllers.Companies;

public record AddCompanyRequest(string Name, List<EmployeeToAdd> Employees);