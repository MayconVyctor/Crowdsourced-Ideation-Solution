using Cis.Core.Application.Services;
using Cis.Core.Domain.Entities;
using Cis.Core.Domain.Repositories;

namespace Cis.UnitTests.Application;

public class TopicQueryServiceTests
{
    [Fact]
    public async Task ReadTopicsAsync_ShouldReturnTopicsNewestFirst()
    {
        // Arrange
        var olderTopic = CreateTopic(1, "Tópico antigo",
            new DateTime(2026, 1, 10));

        var newerTopic = CreateTopic(2, "Tópico recente",
            new DateTime(2026, 5, 20));

        var repository = new FakeTopicRepository(
            [olderTopic, newerTopic]);

        var service = new TopicQueryService(repository);

        // Act
        var result = await service.ReadTopicsAsync();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal((ulong)2, result[0].Id);
        Assert.Equal((ulong)1, result[1].Id);
    }

    [Fact]
    public async Task ReadTopicsAsync_WhenNoTopicsExist_ShouldReturnEmptyList()
    {
        // Arrange
        var repository = new FakeTopicRepository([]);
        var service = new TopicQueryService(repository);

        // Act
        var result = await service.ReadTopicsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    private static Topic CreateTopic(
        ulong id,
        string title,
        DateTime createdAt)
    {
        return new Topic
        {
            Id = id,
            Title = title,
            Description = "Descrição de teste",
            Status = "open",
            IdeaCount = 0,
            Owner = new User
            {
                Id = 10,
                Login = "ellen"
            },
            CreatedAt = createdAt,
            UpdatedAt = createdAt
        };
    }

    private sealed class FakeTopicRepository(
        List<Topic> topics) : ITopicRepository
    {
        public Task<Topic> CreateTopicAsync(Topic topic)
        {
            throw new NotImplementedException();
        }

        public Task<List<Topic>> ReadTopicsAsync()
        {
            return Task.FromResult(topics);
        }

        public Task<Topic?> ReadTopicByIdAsync(ulong id)
        {
            var topic = topics.FirstOrDefault(topic => topic.Id == id);
            return Task.FromResult(topic);
        }

        public Task<Topic> UpdateTopicAsync(Topic topic)
        {
            var index = topics.FindIndex(t => t.Id == topic.Id);
            if (index >= 0)
            {
                topics[index] = topic;
            }
            return Task.FromResult(topic);
        }
    }
}
