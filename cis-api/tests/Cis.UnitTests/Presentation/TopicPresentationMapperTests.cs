using Cis.Core.Domain.Entities;
using Cis.Presentation.Mappers;
using FluentAssertions;
using Xunit;

namespace Cis.UnitTests.Presentation;

public class TopicPresentationMapperTests
{
    [Fact]
    public void ToResponse_WhenValidTopic_ShouldMapAllFieldsCorrectly()
    {

        var now = DateTime.UtcNow;
        var topic = new Topic
        {
            Id = 10,
            Title = "Clean Architecture",
            Description = "Estudo de arquitetura",
            Status = "open",
            UserId = 42,
            IdeaCount = 5,
            CreatedAt = now,
            UpdatedAt = now
        };

        var response = TopicPresentationMapper.ToResponse(topic, ownerLogin: "maycon");

        response.Should().NotBeNull();
        response.Id.Should().Be(10);
        response.Title.Should().Be("Clean Architecture");
        response.Description.Should().Be("Estudo de arquitetura");
        response.Status.Should().Be("open");
        response.IdeaCount.Should().Be(5);
        response.Owner.Id.Should().Be(42);
        response.Owner.Login.Should().Be("maycon");
        response.CreatedAt.Should().Be(now);
        response.UpdatedAt.Should().Be(now);
    }
}