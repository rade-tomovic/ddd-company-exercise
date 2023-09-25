using CompanyManager.Persistence.Domain.Companies;
using CompanyManager.Persistence.Domain.Employees;
using Microsoft.EntityFrameworkCore;

namespace CompanyManager.Persistence;

public class CompaniesDbContext : DbContext
{
    public CompaniesDbContext(DbContextOptions<CompaniesDbContext> options) : base(options) { }

    public DbSet<CompanyDbEntity> Companies { get; set; }
    public DbSet<EmployeeDbEntity> Employees { get; set; }
    public DbSet<CompanyEmployeeDbEntity> CompanyEmployees { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CompaniesDbContext).Assembly);
    }
}