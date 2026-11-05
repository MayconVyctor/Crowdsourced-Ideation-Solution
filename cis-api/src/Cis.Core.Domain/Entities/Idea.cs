namespace Cis.Core.Domain.Entities;

public class Idea
{
    public ulong Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public ulong UserId { get; set; }

    public ulong TopicId { get; set; }

    public User? Owner { get; set; }

    public int VoteCount { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
