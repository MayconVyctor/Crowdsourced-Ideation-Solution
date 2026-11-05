using MongoDB.Bson.Serialization.Attributes;

namespace Cis.Infrastructure.MongoDb.Documents;

/// <summary>
/// Representa um contador de identificadores mantido no MongoDB.
/// A estrutura é específica da infraestrutura de persistência.
/// </summary>
public sealed class CounterDocument
{
    [BsonId]
    public string Id { get; set; } = string.Empty;

    [BsonElement("sequence_value")]
    public long SequenceValue { get; set; }
}
