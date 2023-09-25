using AutoMapper;
using CompanyManager.Domain.Companies.Employees;
using Microsoft.EntityFrameworkCore;

namespace CompanyManager.Persistence.Domain.Employees;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly CompaniesDbContext _context;
    private readonly IMapper _mapper;

    public EmployeeRepository(CompaniesDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<EmployeeId> AddAsync(Employee employee)
    {
        var employeeDbEntity = _mapper.Map<EmployeeDbEntity>(employee);
        await _context.Employees.AddAsync(employeeDbEntity);
        await _context.SaveChangesAsync();

        return new EmployeeId(employeeDbEntity.Id);
    }

    public async Task<Employee> GetByIdAsync(Guid employeeId)
    {
        EmployeeDbEntity employeeDbEntity = await _context.Employees.SingleAsync(x => x.Id == employeeId);

        return _mapper.Map<Employee>(employeeDbEntity);
    }

    public async Task<List<Employee>> GetByIdsAsync(Guid[] employeeIds)
    {
        List<EmployeeDbEntity> employeeDbEntities = await _context.Employees
            .Where(e => employeeIds.Contains(e.Id))
            .ToListAsync();

        return _mapper.Map<List<Employee>>(employeeDbEntities);
    }
}