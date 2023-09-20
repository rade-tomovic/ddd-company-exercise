using MongoDB.Driver;

namespace CompanyManager.Persistence.Domain.SystemLogs;

public class SystemLogsContext
{
    private readonly IMongoDatabase _database;

    public SystemLogsContext(IMongoClient mongoClient)
    {
        _database = mongoClient.GetDatabase("SystemLogDb");
    }

    public IMongoCollection<SystemLogDbEntity> SystemLogs => _database.GetCollection<SystemLogDbEntity>("SystemLogs");
}