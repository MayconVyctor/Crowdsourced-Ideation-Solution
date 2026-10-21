namespace Cis.Infrastructure.MySql.Entities;

public class Vote
{
    public ulong Id { get; set; }

    public ulong UserId { get; set; }

    public ulong TopicId { get; set; }

    public ulong IdeaId { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public User User { get; set; } = null!;

    public Topic Topic { get; set; } = null!;

    public Idea Idea { get; set; } = null!;
}
