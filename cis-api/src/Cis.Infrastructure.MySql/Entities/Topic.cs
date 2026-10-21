namespace Cis.Infrastructure.MySql.Entities;

public class Topic
{
    public ulong Id { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Status { get; set; } = "open";

    public ulong UserId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public User User { get; set; } = null!;

    public ICollection<Idea> Ideas { get; set; } = [];

    public ICollection<Vote> Votes { get; set; } = [];
}
