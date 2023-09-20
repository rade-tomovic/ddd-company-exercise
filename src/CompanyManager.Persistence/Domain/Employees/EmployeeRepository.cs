using CompanyManager.Domain.Companies.Employees;

namespace CompanyManager.Persistence.Domain.Employees;

public class EmployeeRepository : IEmployeeRepository
{
    public async Task<EmployeeId> AddAsync(Employee employee)
    {
        throw new NotImplementedException();
    }

    public async Task<Employee> GetByIdAsync(Guid employeeId)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Employee>> GetByIdsAsync(Guid[] employeeId)
    {
        throw new NotImplementedException();
    }
}