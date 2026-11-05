using DomainIdea = Cis.Core.Domain.Entities.Idea;
using DomainUser = Cis.Core.Domain.Entities.User;
using MySqlIdea = Cis.Infrastructure.MySql.Entities.Idea;

namespace Cis.Infrastructure.MySql.Mappers;

public static class IdeaInfrastructureMapper
{
    public static MySqlIdea ToMySqlEntity(DomainIdea domain)
    {
        ArgumentNullException.ThrowIfNull(domain);

        return new MySqlIdea
        {
            Id = domain.Id,
            Title = domain.Title,
            Description = domain.Description,
            UserId = domain.UserId,
            TopicId = domain.TopicId,
            CreatedAt = domain.CreatedAt,
            UpdatedAt = domain.UpdatedAt
        };
    }

    public static DomainIdea ToDomainEntity(MySqlIdea entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        if (entity.User is null)
        {
            throw new InvalidOperationException(
                "The idea must have its owner loaded.");
        }

        return new DomainIdea
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description,
            UserId = entity.UserId,
            TopicId = entity.TopicId ?? 0,
            Owner = new DomainUser
            {
                Id = entity.User.Id,
                Login = entity.User.Login
            },
            VoteCount = entity.Votes.Count(vote => vote.IsActive),
            CreatedAt = entity.CreatedAt
                ?? throw new InvalidOperationException(
                    "The idea creation date cannot be null."),
            UpdatedAt = entity.UpdatedAt
                ?? throw new InvalidOperationException(
                    "The idea update date cannot be null.")
        };
    }
}
