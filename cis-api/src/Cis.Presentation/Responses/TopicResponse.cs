namespace Cis.Presentation.Responses;

public record TopicResponse(
    ulong Id,
    string Title,
    string Description,
    string Status,
    int IdeaCount,
    UserResponse Owner,
    DateTime CreatedAt,
    DateTime UpdatedAt);

