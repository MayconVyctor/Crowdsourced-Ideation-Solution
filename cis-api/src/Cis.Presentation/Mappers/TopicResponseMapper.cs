using Cis.Core.Domain.Entities;
using Cis.Presentation.Responses;

namespace Cis.Presentation.Mappers;

public static class TopicResponseMapper
{
    public static TopicResponse ToResponse(Topic topic)
    {
        ArgumentNullException.ThrowIfNull(topic);

        var ownerId = topic.Owner?.Id ?? topic.UserId;
        var ownerLogin = topic.Owner?.Login ?? "system";

        return new TopicResponse(
            topic.Id,
            topic.Title,
            topic.Description,
            topic.Status,
            topic.IdeaCount,
            new UserResponse(ownerId, ownerLogin),
            topic.CreatedAt,
            topic.UpdatedAt);
    }
}
