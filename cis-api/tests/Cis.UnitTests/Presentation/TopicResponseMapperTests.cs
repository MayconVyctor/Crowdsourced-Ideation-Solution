using Cis.Core.Domain.Entities;
using Cis.Presentation.Mappers;

namespace Cis.UnitTests.Presentation;

public class TopicResponseMapperTests
{
    [Fact]
    public void ToResponse_ShouldMapAllTopicProperties()
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
            IdeaCount = 3,
            Owner = new User
            {
                Id = 10,
                Login = "ellen"
            },
            CreatedAt = createdAt,
            UpdatedAt = updatedAt
        };

        // Act
        var result = TopicResponseMapper.ToResponse(topic);

        // Assert
        Assert.Equal((ulong)1, result.Id);
        Assert.Equal("Topic title", result.Title);
        Assert.Equal("Topic description", result.Description);
        Assert.Equal("open", result.Status);
        Assert.Equal(3, result.IdeaCount);
        Assert.NotNull(result.Owner);
        Assert.Equal((ulong)10, result.Owner.Id);
        Assert.Equal("ellen", result.Owner.Login);
        Assert.Equal(createdAt, result.CreatedAt);
        Assert.Equal(updatedAt, result.UpdatedAt);
    }

    [Fact]
    public void ToResponse_WhenStatusIsClosed_ShouldMapClosedStatus()
    {
        // Arrange
        var topic = new Topic
        {
            Id = 2,
            Title = "Closed topic",
            Description = "Closed description",
            Status = "closed",
            IdeaCount = 0,
            UserId = 10,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Act
        var result = TopicResponseMapper.ToResponse(topic);

        // Assert
        Assert.Equal("closed", result.Status);
    }

    [Fact]
    public void ToResponse_WhenTopicIsNull_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(
            () => TopicResponseMapper.ToResponse(null!));
    }
}
