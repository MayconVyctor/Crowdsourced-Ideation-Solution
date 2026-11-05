namespace Cis.Presentation.Responses;

public record IdeaResponse(
    ulong Id,
    ulong TopicId,
    string Title,
    string Description,
    int VoteCount,
    UserResponse Owner,
    DateTime CreatedAt,
    DateTime UpdatedAt);
