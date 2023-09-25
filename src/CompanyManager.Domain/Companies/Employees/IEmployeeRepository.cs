namespace CompanyManager.Domain.Companies.Employees;

public interface IEmployeeRepository
{
    Task<EmployeeId> AddAsync(Employee employee);
    Task<Employee> GetByIdAsync(Guid employeeId);
    Task<List<Employee>> GetByIdsAsync(Guid[] employeeIds);
}