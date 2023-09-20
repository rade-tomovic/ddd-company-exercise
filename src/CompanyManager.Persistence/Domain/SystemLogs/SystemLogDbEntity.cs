using System.Text.Json;
using System;
using MongoDB.Bson;

namespace CompanyManager.Persistence.Domain.SystemLogs;

public class SystemLogDbEntity
{
    public ObjectId Id { get; set; }
    public string Comment { get; set; }
    public string Event { get; set; }
    public string ResourceType { get; set; }
    public DateTime CreatedAt { get; set; }
    public BsonDocument Entity { get; set; }
}