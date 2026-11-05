using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Cis.Infrastructure.MongoDb.Documents;

/// <summary>
/// Representa um tópico armazenado no MongoDB.
/// Os nomes dos campos e a versão do esquema são específicos da persistência.
/// </summary>
public sealed class TopicDocument
{
    public const int CurrentSchemaVersion = 1;

    [BsonId]
    public long Id { get; set; }

    [BsonElement("title")]
    public string Title { get; set; } = string.Empty;

    [BsonElement("description")]
    public string Description { get; set; } = string.Empty;

    [BsonElement("status")]
    public string Status { get; set; } = "open";

    [BsonElement("user_id")]
    public long UserId { get; set; }

    [BsonElement("idea_count")]
    public int IdeaCount { get; set; }

    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; }

    [BsonElement("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [BsonElement("schema_version")]
    public int SchemaVersion { get; set; } = CurrentSchemaVersion;
}
