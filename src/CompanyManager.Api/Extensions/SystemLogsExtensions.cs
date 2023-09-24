using CompanyManager.Application.Shared;
using CompanyManager.Domain.SystemLogs;
using CompanyManager.Persistence.Domain.SystemLogs;
using MongoDB.Driver;

namespace CompanyManager.Api.Extensions;

public static class SystemLogsExtensions
{
    public static IServiceCollection AddSystemLogHandling(this IServiceCollection services, IConfiguration configuration)
    {
        var url = new MongoUrl(configuration["DbConnections:SystemLogDb"]);
        services.AddSingleton<IMongoClient>(new MongoClient(url));
        services.AddScoped<ISystemLogRepository, SystemLogRepository>();
        services.AddScoped<ISystemLogPublisher, SystemLogPublisher>();

        return services;
    }
}