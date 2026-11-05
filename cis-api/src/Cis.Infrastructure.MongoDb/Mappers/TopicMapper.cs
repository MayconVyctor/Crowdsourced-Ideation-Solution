using Cis.Infrastructure.MongoDb.Documents;
using DomainTopic = Cis.Core.Domain.Entities.Topic;

namespace Cis.Infrastructure.MongoDb.Mappers;

/// <summary>
/// Converte Topic entre o modelo de domínio e o documento MongoDB.
/// Não acessa banco de dados nem executa regras de negócio.
/// </summary>
public static class TopicMapper
{
    public static TopicDocument ToDocument(DomainTopic domain)
    {
        ArgumentNullException.ThrowIfNull(domain);

        return new TopicDocument
        {
            Id = checked((long)domain.Id),
            Title = domain.Title,
            Description = domain.Description,
            Status = domain.Status,
            UserId = checked((long)domain.UserId),
            IdeaCount = domain.IdeaCount,
            CreatedAt = domain.CreatedAt,
            UpdatedAt = domain.UpdatedAt,
            SchemaVersion = TopicDocument.CurrentSchemaVersion
        };
    }

    public static DomainTopic ToDomain(TopicDocument document)
    {
        ArgumentNullException.ThrowIfNull(document);

        return new DomainTopic
        {
            Id = checked((ulong)document.Id),
            Title = document.Title,
            Description = document.Description,
            Status = document.Status,
            UserId = checked((ulong)document.UserId),
            IdeaCount = document.IdeaCount,
            CreatedAt = document.CreatedAt,
            UpdatedAt = document.UpdatedAt
        };
    }
}
