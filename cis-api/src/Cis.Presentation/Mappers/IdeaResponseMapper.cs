using Cis.Core.Domain.Entities;
using Cis.Presentation.Responses;

namespace Cis.Presentation.Mappers;

public static class IdeaResponseMapper
{
    public static IdeaResponse ToResponse(Idea idea)
    {
        ArgumentNullException.ThrowIfNull(idea);

        var ownerId = idea.Owner?.Id ?? idea.UserId;
        var ownerLogin = idea.Owner?.Login ?? "system";

        return new IdeaResponse(
            idea.Id,
            idea.TopicId,
            idea.Title,
            idea.Description,
            idea.VoteCount,
            new UserResponse(ownerId, ownerLogin),
            idea.CreatedAt,
            idea.UpdatedAt);
    }
}
