using Cis.Core.Domain.Entities;
using Cis.Presentation.Responses;

namespace Cis.Presentation.Mappers;

public static class TopicPresentationMapper
{
    public static TopicResponse ToResponse(Topic topic, string ownerLogin = "system")
    {
        var owner = new UserResponse(topic.UserId, ownerLogin);

        return new TopicResponse(
            topic.Id,
            topic.Title,
            topic.Description,
            topic.Status,
            topic.IdeaCount,
            owner,
            topic.CreatedAt,
            topic.UpdatedAt);
    }
}