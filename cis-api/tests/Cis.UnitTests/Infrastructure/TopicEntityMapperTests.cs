using Cis.Core.Domain.Entities;
using Cis.Infrastructure.MySql.Mappers;
using MySqlIdea = Cis.Infrastructure.MySql.Entities.Idea;
using MySqlTopic = Cis.Infrastructure.MySql.Entities.Topic;
using MySqlUser = Cis.Infrastructure.MySql.Entities.User;

namespace Cis.UnitTests.Infrastructure;

public class TopicEntityMapperTests
{
    [Fact]
    public void ToDomain_ShouldMapTopicAndCountIdeas()
    {
        // Arrange
        var createdAt = new DateTime(2026, 1, 10, 10, 0, 0);
        var updatedAt = new DateTime(2026, 5, 20, 15, 0, 0);

        var entity = new MySqlTopic
        {
            Id = 1,
            Title = "Topic title",
            Description = "Topic description",
            Status = "open",
            UserId = 10,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt,
            User = new MySqlUser
            {
                Id = 10,
                SourceId = "source-10",
                Login = "ellen"
            },
            Ideas =
            [
                new(),
                new()
            ]
        };

        // Act
        var result = TopicEntityMapper.ToDomain(entity);

        // Assert
        Assert.Equal((ulong)1, result.Id);
        Assert.Equal("Topic title", result.Title);
        Assert.Equal("Topic description", result.Description);
        Assert.Equal("open", result.Status);
        Assert.Equal(2, result.IdeaCount);
        Assert.NotNull(result.Owner);
        Assert.Equal((ulong)10, result.Owner.Id);
        Assert.Equal("ellen", result.Owner.Login);
        Assert.Equal(createdAt, result.CreatedAt);
        Assert.Equal(updatedAt, result.UpdatedAt);
    }

    [Fact]
    public void ToDomain_WhenTopicIsClosed_ShouldMapClosedStatus()
    {
        // Arrange
        var createdAt = new DateTime(2026, 1, 10, 10, 0, 0);
        var updatedAt = new DateTime(2026, 5, 20, 15, 0, 0);

        var entity = new MySqlTopic
        {
            Id = 5,
            Title = "Closed Topic",
            Description = "Closed description",
            Status = "closed",
            UserId = 10,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt,
            User = new MySqlUser
            {
                Id = 10,
                SourceId = "source-10",
                Login = "owner"
            },
            Ideas = []
        };

        // Act
        var result = TopicEntityMapper.ToDomain(entity);

        // Assert
        Assert.Equal("closed", result.Status);
        Assert.Equal(updatedAt, result.UpdatedAt);
    }

    [Fact]
    public void ToEntity_ShouldMapDomainTopic()
    {
        // Arrange
        var createdAt = new DateTime(2026, 1, 10, 10, 0, 0);
        var updatedAt = new DateTime(2026, 5, 20, 15, 0, 0);

        var topic = new Topic
        {
            Id = 1,
            Title = "Topic title",
            Description = "Topic description",
            Status = "open",
            IdeaCount = 2,
            Owner = new User
            {
                Id = 10,
                Login = "ellen"
            },
            CreatedAt = createdAt,
            UpdatedAt = updatedAt
        };

        // Act
        var result = TopicEntityMapper.ToEntity(topic);

        // Assert
        Assert.Equal((ulong)1, result.Id);
        Assert.Equal("Topic title", result.Title);
        Assert.Equal("Topic description", result.Description);
        Assert.Equal("open", result.Status);
        Assert.Equal((ulong)10, result.UserId);
        Assert.Equal(createdAt, result.CreatedAt);
        Assert.Equal(updatedAt, result.UpdatedAt);
    }

    [Fact]
    public void ToEntity_WhenTopicIsClosed_ShouldMapClosedStatus()
    {
        // Arrange
        var createdAt = new DateTime(2026, 1, 10, 10, 0, 0);
        var updatedAt = new DateTime(2026, 5, 20, 15, 0, 0);

        var topic = new Topic
        {
            Id = 5,
            Title = "Closed Topic",
            Description = "Closed description",
            Status = "closed",
            UserId = 10,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt
        };

        // Act
        var result = TopicEntityMapper.ToEntity(topic);

        // Assert
        Assert.Equal("closed", result.Status);
        Assert.Equal(updatedAt, result.UpdatedAt);
    }

    [Fact]
    public void ToDomain_WhenEntityIsNull_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(
            () => TopicEntityMapper.ToDomain(null!));
    }

    [Fact]
    public void ToEntity_WhenTopicIsNull_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(
            () => TopicEntityMapper.ToEntity(null!));
    }
}
