using CompanyManager.Domain.Companies.Employees;

namespace CompanyManager.Api.Controllers.Employees;

public record AddEmployeeRequest(List<Guid> CompanyIds, EmployeeTitle Title, string Email);