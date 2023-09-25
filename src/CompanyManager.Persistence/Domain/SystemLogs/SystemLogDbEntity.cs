using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CompanyManager.Persistence.Domain.SystemLogs;

public class SystemLogDbEntity
{
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement("_id")]
    public string Id { get; set; }
    public string Comment { get; set; }
    public string Event { get; set; }
    public string ResourceType { get; set; }
    public DateTime CreatedAt { get; set; }
    public BsonDocument Entity { get; set; }
}