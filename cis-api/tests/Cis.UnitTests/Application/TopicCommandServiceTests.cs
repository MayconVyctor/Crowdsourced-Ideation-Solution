using Cis.Core.Application.Services;
using Cis.Core.Domain.Entities;
using Cis.Core.Domain.Exceptions;
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

    [Fact]
    public async Task CloseTopicAsync_WhenTopicExistsAndUserIsOwner_ShouldCloseTopicSuccessfully()
    {
        // Arrange
        var repositoryMock = new Mock<ITopicRepository>();
        var existingTopic = new Topic
        {
            Id = 1,
            Title = "Topic 1",
            Description = "Description 1",
            UserId = 42,
            Status = "open",
            CreatedAt = DateTime.UtcNow.AddHours(-1),
            UpdatedAt = DateTime.UtcNow.AddHours(-1)
        };

        repositoryMock
            .Setup(r => r.ReadTopicByIdAsync(1))
            .ReturnsAsync(existingTopic);

        repositoryMock
            .Setup(r => r.UpdateTopicAsync(It.IsAny<Topic>()))
            .ReturnsAsync((Topic t) => t);

        var service = new TopicCommandService(repositoryMock.Object);

        // Act
        var result = await service.CloseTopicAsync(1, 42);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        result.Status.Should().Be("closed");

        repositoryMock.Verify(r => r.ReadTopicByIdAsync(1), Times.Once);
        repositoryMock.Verify(r => r.UpdateTopicAsync(It.Is<Topic>(t =>
            t.Id == 1 &&
            t.Status == "closed" &&
            t.UserId == 42)), Times.Once);
    }

    [Fact]
    public async Task CloseTopicAsync_WhenTopicAlreadyClosed_ShouldReturnTopicWithoutUpdating()
    {
        // Arrange
        var repositoryMock = new Mock<ITopicRepository>();
        var existingTopic = new Topic
        {
            Id = 1,
            Title = "Topic 1",
            Description = "Description 1",
            UserId = 42,
            Status = "closed",
            CreatedAt = DateTime.UtcNow.AddHours(-2),
            UpdatedAt = DateTime.UtcNow.AddHours(-1)
        };

        repositoryMock
            .Setup(r => r.ReadTopicByIdAsync(1))
            .ReturnsAsync(existingTopic);

        var service = new TopicCommandService(repositoryMock.Object);

        // Act
        var result = await service.CloseTopicAsync(1, 42);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        result.Status.Should().Be("closed");

        repositoryMock.Verify(r => r.ReadTopicByIdAsync(1), Times.Once);
        repositoryMock.Verify(r => r.UpdateTopicAsync(It.IsAny<Topic>()), Times.Never);
    }

    [Fact]
    public async Task CloseTopicAsync_WhenTopicNotFound_ShouldThrowTopicNotFoundException()
    {
        // Arrange
        var repositoryMock = new Mock<ITopicRepository>();
        repositoryMock
            .Setup(r => r.ReadTopicByIdAsync(999))
            .ReturnsAsync((Topic?)null);

        var service = new TopicCommandService(repositoryMock.Object);

        // Act
        var act = () => service.CloseTopicAsync(999, 42);

        // Assert
        await act.Should().ThrowAsync<TopicNotFoundException>()
            .WithMessage("Topic with ID 999 was not found.");

        repositoryMock.Verify(r => r.UpdateTopicAsync(It.IsAny<Topic>()), Times.Never);
    }

    [Fact]
    public async Task CloseTopicAsync_WhenUserIsNotOwner_ShouldThrowTopicForbiddenException()
    {
        // Arrange
        var repositoryMock = new Mock<ITopicRepository>();
        var existingTopic = new Topic
        {
            Id = 1,
            Title = "Topic 1",
            Description = "Description 1",
            UserId = 42,
            Status = "open"
        };

        repositoryMock
            .Setup(r => r.ReadTopicByIdAsync(1))
            .ReturnsAsync(existingTopic);

        var service = new TopicCommandService(repositoryMock.Object);

        // Act
        var act = () => service.CloseTopicAsync(1, 99);

        // Assert
        await act.Should().ThrowAsync<TopicForbiddenException>()
            .WithMessage("User with ID 99 is not allowed to close topic with ID 1.");

        repositoryMock.Verify(r => r.UpdateTopicAsync(It.IsAny<Topic>()), Times.Never);
    }
}
