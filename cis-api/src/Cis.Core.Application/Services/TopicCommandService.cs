using Cis.Core.Domain.Entities;
using Cis.Core.Domain.Exceptions;
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

    public async Task<Topic> CloseTopicAsync(ulong topicId, ulong userId)
    {
        var topic = await topicRepository.ReadTopicByIdAsync(topicId)
            ?? throw new TopicNotFoundException($"Topic with ID {topicId} was not found.");

        if (topic.UserId != userId)
        {
            throw new TopicForbiddenException($"User with ID {userId} is not allowed to close topic with ID {topicId}.");
        }

        if (string.Equals(topic.Status, "closed", StringComparison.OrdinalIgnoreCase))
        {
            return topic;
        }

        topic.Status = "closed";
        topic.UpdatedAt = DateTime.UtcNow;

        return await topicRepository.UpdateTopicAsync(topic);
    }
}
