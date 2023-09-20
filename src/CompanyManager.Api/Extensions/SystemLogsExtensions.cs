using MongoDB.Driver;

namespace CompanyManager.Api.Extensions;

public static class SystemLogsExtensions
{
    public static IServiceCollection AddSystemLogHandling(this IServiceCollection services, IConfiguration configuration)
    {
        var mongoSettings = new MongoClientSettings
        {
            Server = new MongoServerAddress(configuration["DbConnections:SystemLogDb"])
        };
        services.AddSingleton<IMongoClient>(new MongoClient(mongoSettings));

        return services;
    }
}