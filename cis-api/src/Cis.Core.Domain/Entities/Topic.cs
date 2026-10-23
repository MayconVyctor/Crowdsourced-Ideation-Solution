namespace Cis.Core.Domain.Entities;

public class Topic
{
    public ulong Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = "open";
    public ulong UserId { get; set; }
    public int IdeaCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}