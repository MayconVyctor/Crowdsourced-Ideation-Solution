using Cis.Core.Domain.Entities;
using Cis.Core.Domain.Repositories;

namespace Cis.Core.Application.Services;

public class TopicCommandService(ITopicRepository topicRepository) : ITopicCommandService
{
    public async Task<Topic> CreateTopicAsync(string title, string description, ulong userId)
    {
        var topic = new Topic
        {
            Title = title,
            Description = description,
            UserId = userId,
            Status = "open",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        return await topicRepository.CreateTopicAsync(topic);
    }
}