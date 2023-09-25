using System.Text.Json;
using System.Text.Json.Serialization;
using AutoMapper;
using CompanyManager.Domain.Shared.Contracts;
using CompanyManager.Domain.Shared.Core;
using CompanyManager.Domain.SystemLogs;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CompanyManager.Persistence.Domain.SystemLogs;

public class SystemLogRepository : ISystemLogRepository
{
    private readonly IMongoClient _mongoClient;

    public SystemLogRepository(IMongoClient mongoClient, IMapper mapper)
    {
        _mongoClient = mongoClient;
    }

    public async Task<string> AddSystemLog(SystemLog<IDomainEvent<Entity>, Entity> log)
    {
        IMongoCollection<SystemLogDbEntity>? collection = _mongoClient
            .GetDatabase("SystemLogDb").GetCollection<SystemLogDbEntity>("SystemLogs");

        var dbEntity = new SystemLogDbEntity
        {
            Comment = log.Comment,
            CreatedAt = log.CreatedAt,
            Entity = BsonDocument.Parse(JsonSerializer.Serialize(log.Entity, log.Entity.GetType(), new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                Converters = { new JsonStringEnumConverter() }
            })),
            Event = log.Event,
            ResourceType = log.ResourceType
        };

        await collection.InsertOneAsync(dbEntity);

        return dbEntity.Id;
    }
}