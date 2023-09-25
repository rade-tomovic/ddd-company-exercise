using AutoMapper;
using CompanyManager.Domain.Companies;
using CompanyManager.Domain.Companies.Employees;
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

        return companies.Select(x => _mapper.Map<Company>(x));
    }

    public async Task<bool> UpdateAsync(Company company)
    {
        CompanyDbEntity? companyDbEntity = await _context.Companies.FindAsync(company.Id);

        if (companyDbEntity == null)
            return false;

        _mapper.Map(company, companyDbEntity);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> IsNameUniqueAsync(string name)
    {
        return !await _context.Companies.AnyAsync(c => c.Name == name);
    }

    public async Task<bool> IsEmployeeEmailUniqueAsync(string email, Guid companyId)
    {
        return !await _context.CompanyEmployees.AnyAsync(ce => ce.CompanyId == companyId && ce.Employee.Email == email);
    }

    public async Task<bool> IsEmployeeTitleWithinCompanyUniqueAsync(EmployeeTitle title, Guid companyId)
    {
        return !await _context.CompanyEmployees.AnyAsync(ce => ce.CompanyId == companyId && ce.Employee.Title == title.ToString());
    }
}