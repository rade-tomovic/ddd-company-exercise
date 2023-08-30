namespace CompanyManager.Domain.Companies.Employees;

public interface IEmployeeRepository
{
    Task<EmployeeId> AddAsync(Employee employee);
}