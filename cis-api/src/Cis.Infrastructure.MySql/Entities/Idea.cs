namespace Cis.Infrastructure.MySql.Entities;

public class Idea
{
    public ulong Id { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public ulong UserId { get; set; }

    public ulong? TopicId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public User User { get; set; } = null!;

    public Topic? Topic { get; set; }

    public ICollection<Vote> Votes { get; set; } = [];
}
