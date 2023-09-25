using CompanyManager.Application.Companies.DomainServices;
using CompanyManager.Domain.Companies.Contracts;

namespace CompanyManager.Api.Extensions;

public static class DomainServicesExtensions
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddScoped<ICompanyUniquenessChecker, CompanyUniquenessChecker>();
        services.AddScoped<IEmployeeEmailUniquenessChecker, EmployeeEmailUniquenessChecker>();
        services.AddScoped<IEmployeeTitleWithinCompanyUniquenessChecker, EmployeeTitleWithinCompanyUniquenessChecker>();

        return services;
    }
}