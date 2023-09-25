using AutoMapper;
using CompanyManager.Domain.Companies;
using CompanyManager.Domain.Companies.Employees;
using CompanyManager.Persistence.Domain.Employees;
using Microsoft.EntityFrameworkCore;

namespace CompanyManager.Persistence.Domain.Companies;

public class CompanyRepository : ICompanyRepository
{
    private readonly CompaniesDbContext _context;
    private readonly IMapper _mapper;

    public CompanyRepository(CompaniesDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CompanyId> AddAsync(Company company)
    {
        var companyDbEntity = _mapper.Map<CompanyDbEntity>(company);
        await _context.Companies.AddAsync(companyDbEntity);
        await _context.SaveChangesAsync();

        return new CompanyId(companyDbEntity.Id);
    }

    public async Task<IEnumerable<Company>> GetByIdsAsync(List<Guid> companyIds)
    {
        List<CompanyDbEntity> companies = await _context.Companies
            .Where(c => companyIds.Contains(c.Id))
            .AsNoTracking()
            .ToListAsync();

        return companies.Select(x => Company.CreateNewWithoutChecking(x.Name, x.CreatedAt, x.Id));
    }

    public async Task<bool> AddEmployeeToCompany(Company company)
    {
        CompanyDbEntity? companyDbEntity = await _context.Companies.SingleOrDefaultAsync(x => x.Id == company.Id);

        if (companyDbEntity == null)
            throw new ArgumentNullException(nameof(companyDbEntity), $"Cannot find company with ID: {company.Id}");

        foreach (Employee? employee in company.Employees)
        {
            var employeeToAdd = new EmployeeDbEntity()
            {
                Email = employee.Email,
                CreatedAt = employee.CreatedAt,
                Title = employee.Title.ToString()
            };

            await _context.Employees.AddAsync(employeeToAdd);

            companyDbEntity.Employees.Add(employeeToAdd); 
        }
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> IsNameUniqueAsync(string name)
    {
        return !await _context.Companies.AnyAsync(c => c.Name == name);
    }

    public async Task<bool> IsEmployeeEmailUniqueAsync(string email, Guid companyId)
    {
        return !await _context.Employees.AnyAsync(ce => ce.Companies.Any(x => x.Id == companyId) && ce.Email == email);
    }

    public async Task<bool> IsEmployeeTitleWithinCompanyUniqueAsync(EmployeeTitle title, Guid companyId)
    {
        return !await _context.Employees.AnyAsync(ce => ce.Companies.Any(x => x.Id == companyId) && ce.Title == title.ToString());
    }
}