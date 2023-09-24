using CompanyManager.Domain.Companies.Employees;

namespace CompanyManager.Api.Controllers;

public record AddEmployeeRequest(List<Guid> CompanyIds, EmployeeTitle Title, string Email);