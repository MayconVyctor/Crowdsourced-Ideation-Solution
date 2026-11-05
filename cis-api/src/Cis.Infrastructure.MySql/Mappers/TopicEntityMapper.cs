using DomainTopic = Cis.Core.Domain.Entities.Topic;
using DomainUser = Cis.Core.Domain.Entities.User;
using MySqlTopic = Cis.Infrastructure.MySql.Entities.Topic;

namespace Cis.Infrastructure.MySql.Mappers;

public static class TopicEntityMapper
{
    public static DomainTopic ToDomain(MySqlTopic entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        if (entity.User is null)
        {
            throw new InvalidOperationException(
                "The topic must have its owner loaded.");
        }

        return new DomainTopic
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description,
            Status = entity.Status,
            UserId = entity.UserId,
            IdeaCount = entity.Ideas.Count,
            Owner = new DomainUser
            {
                Id = entity.User.Id,
                Login = entity.User.Login
            },
            CreatedAt = entity.CreatedAt
                ?? throw new InvalidOperationException(
                    "The topic creation date cannot be null."),
            UpdatedAt = entity.UpdatedAt
                ?? throw new InvalidOperationException(
                    "The topic update date cannot be null.")
        };
    }

    public static MySqlTopic ToEntity(DomainTopic topic)
    {
        ArgumentNullException.ThrowIfNull(topic);

        return new MySqlTopic
        {
            Id = topic.Id,
            Title = topic.Title,
            Description = topic.Description,
            Status = topic.Status,
            UserId = topic.Owner?.Id ?? topic.UserId,
            CreatedAt = topic.CreatedAt,
            UpdatedAt = topic.UpdatedAt
        };
    }
}
