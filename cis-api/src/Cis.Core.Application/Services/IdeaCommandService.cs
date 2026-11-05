using Cis.Core.Domain.Entities;
using Cis.Core.Domain.Exceptions;
using Cis.Core.Domain.Repositories;

namespace Cis.Core.Application.Services;

public class IdeaCommandService(
    ITopicRepository topicRepository,
    IUserRepository userRepository,
    IIdeaRepository ideaRepository) : IIdeaCommandService
{
    public async Task<Idea> CreateIdeaAsync(
        ulong topicId,
        string title,
        string description,
        ulong userId)
    {
        var topic = await topicRepository.ReadTopicByIdAsync(topicId)
            ?? throw new TopicNotFoundException(
                $"Topic with ID {topicId} was not found.");

        if (!string.Equals(
            topic.Status,
            "open",
            StringComparison.OrdinalIgnoreCase))
        {
            throw new TopicClosedException(
                $"Topic with ID {topicId} is closed.");
        }

        var userExists = await userRepository.ExistsAsync(userId);

        if (!userExists)
        {
            throw new UserNotFoundException(
                $"User with ID {userId} was not found.");
        }

        var now = DateTime.UtcNow;

        var idea = new Idea
        {
            Title = title.Trim(),
            Description = description.Trim(),
            UserId = userId,
            TopicId = topicId,
            CreatedAt = now,
            UpdatedAt = now
        };

        return await ideaRepository.CreateIdeaAsync(idea);
    }
}
