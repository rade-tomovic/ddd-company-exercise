using CompanyManager.Domain.Companies;
using CompanyManager.Domain.Companies.Employees;
using CompanyManager.Persistence;
using CompanyManager.Persistence.Domain.Companies;
using CompanyManager.Persistence.Domain.Employees;
using Microsoft.EntityFrameworkCore;

namespace CompanyManager.Api.Extensions;

public static class PersistenceExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CompaniesDbContext>(options =>
        {
            options.UseNpgsql(configuration["DbConnections:CompaniesDb"]);
        });

        services.AddScoped<ICompanyRepository, CompanyRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();

        return services;
    }
}