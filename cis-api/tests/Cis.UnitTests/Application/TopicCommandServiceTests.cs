using Cis.Core.Application.Services;
using Cis.Core.Domain.Entities;
using Cis.Core.Domain.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace Cis.UnitTests.Application;

public class TopicCommandServiceTests
{
    [Fact]
    public async Task CreateTopicAsync_WhenCalled_ShouldInstantiateTopicAndCallRepository()
    {

        var repositoryMock = new Mock<ITopicRepository>();

        var expectedTopic = new Topic
        {
            Id = 1,
            Title = "Docker Optimization",
            Description = "Multi-stage builds",
            UserId = 99,
            Status = "open"
        };

        repositoryMock
            .Setup(r => r.CreateTopicAsync(It.IsAny<Topic>()))
            .ReturnsAsync(expectedTopic);

        var service = new TopicCommandService(repositoryMock.Object);

        var result = await service.CreateTopicAsync("Docker Optimization", "Multi-stage builds", 99);

        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        result.Title.Should().Be("Docker Optimization");
        result.UserId.Should().Be(99);

        repositoryMock.Verify(
            r => r.CreateTopicAsync(It.Is<Topic>(t =>
                t.Title == "Docker Optimization" &&
                t.Description == "Multi-stage builds" &&
                t.UserId == 99 &&
                t.Status == "open")),
            Times.Once);
    }
}