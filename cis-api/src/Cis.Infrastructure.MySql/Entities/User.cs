namespace Cis.Infrastructure.MySql.Entities;

public class User
{
    public ulong Id { get; set; }

    public string SourceId { get; set; } = null!;

    public string Login { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public ICollection<Topic> Topics { get; set; } = [];

    public ICollection<Idea> Ideas { get; set; } = [];

    public ICollection<Vote> Votes { get; set; } = [];
}
