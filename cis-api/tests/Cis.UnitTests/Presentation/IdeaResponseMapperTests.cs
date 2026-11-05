using Cis.Core.Domain.Entities;
using Cis.Presentation.Mappers;

namespace Cis.UnitTests.Presentation;

public class IdeaResponseMapperTests
{
    [Fact]
    public void ToResponse_ShouldMapAllIdeaProperties()
    {
        // Arrange
        var createdAt = new DateTime(2026, 1, 10, 10, 0, 0);
        var updatedAt = new DateTime(2026, 5, 20, 15, 0, 0);

        var idea = new Idea
        {
            Id = 1,
            TopicId = 2,
            Title = "Idea title",
            Description = "Idea description",
            UserId = 10,
            VoteCount = 5,
            Owner = new User
            {
                Id = 10,
                Login = "ellen"
            },
            CreatedAt = createdAt,
            UpdatedAt = updatedAt
        };

        // Act
        var result = IdeaResponseMapper.ToResponse(idea);

        // Assert
        Assert.Equal((ulong)1, result.Id);
        Assert.Equal((ulong)2, result.TopicId);
        Assert.Equal("Idea title", result.Title);
        Assert.Equal("Idea description", result.Description);
        Assert.Equal(5, result.VoteCount);
        Assert.NotNull(result.Owner);
        Assert.Equal((ulong)10, result.Owner.Id);
        Assert.Equal("ellen", result.Owner.Login);
        Assert.Equal(createdAt, result.CreatedAt);
        Assert.Equal(updatedAt, result.UpdatedAt);
    }

    [Fact]
    public void ToResponse_WhenIdeaIsNull_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(
            () => IdeaResponseMapper.ToResponse(null!));
    }
}
