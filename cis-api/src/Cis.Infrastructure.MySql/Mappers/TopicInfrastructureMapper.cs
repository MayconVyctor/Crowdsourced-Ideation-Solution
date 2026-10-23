using DomainTopic = Cis.Core.Domain.Entities.Topic;
using MySqlTopic = Cis.Infrastructure.MySql.Entities.Topic;

namespace Cis.Infrastructure.MySql.Mappers;

public static class TopicInfrastructureMapper
{
    public static MySqlTopic ToMySqlEntity(DomainTopic domain)
    {
        return new MySqlTopic
        {
            Id = domain.Id,
            Title = domain.Title,
            Description = domain.Description,
            Status = domain.Status,
            UserId = domain.UserId,
            CreatedAt = domain.CreatedAt,
            UpdatedAt = domain.UpdatedAt
        };
    }
    public static DomainTopic ToDomainEntity(MySqlTopic entity)
    {
        return new DomainTopic
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description,
            Status = entity.Status,
            UserId = entity.UserId,
            IdeaCount = entity.Ideas?.Count ?? 0,
            CreatedAt = entity.CreatedAt ?? DateTime.UtcNow,
            UpdatedAt = entity.UpdatedAt ?? DateTime.UtcNow
        };
    }
}
