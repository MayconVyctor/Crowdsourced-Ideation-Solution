using Cis.Core.Application.Services;
using Cis.Core.Domain.Entities;
using Cis.Core.Domain.Exceptions;
using Cis.Core.Domain.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace Cis.UnitTests.Application;

public class IdeaCommandServiceTests
{
    [Fact]
    public async Task CreateIdeaAsync_WhenCalled_ShouldCreateIdeaSuccessfully()
    {
        // Arrange
        var topicRepositoryMock = new Mock<ITopicRepository>();
        var userRepositoryMock = new Mock<IUserRepository>();
        var ideaRepositoryMock = new Mock<IIdeaRepository>();

        var topic = new Topic
        {
            Id = 1,
            Status = "open"
        };

        var expectedIdea = new Idea
        {
            Id = 10,
            TopicId = 1,
            Title = "My idea",
            Description = "My description",
            UserId = 99,
            VoteCount = 0
        };

        topicRepositoryMock
            .Setup(r => r.ReadTopicByIdAsync(1))
            .ReturnsAsync(topic);

        userRepositoryMock
            .Setup(r => r.ExistsAsync(99))
            .ReturnsAsync(true);

        ideaRepositoryMock
            .Setup(r => r.CreateIdeaAsync(It.IsAny<Idea>()))
            .ReturnsAsync(expectedIdea);

        var service = new IdeaCommandService(
            topicRepositoryMock.Object,
            userRepositoryMock.Object,
            ideaRepositoryMock.Object);

        // Act
        var result = await service.CreateIdeaAsync(
            1,
            "  My idea  ",
            "  My description  ",
            99);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(10);
        result.Title.Should().Be("My idea");
        result.Description.Should().Be("My description");
        result.TopicId.Should().Be(1);
        result.UserId.Should().Be(99);

        ideaRepositoryMock.Verify(
            r => r.CreateIdeaAsync(It.Is<Idea>(idea =>
                idea.Title == "My idea" &&
                idea.Description == "My description" &&
                idea.TopicId == 1 &&
                idea.UserId == 99)),
            Times.Once);
    }

    [Fact]
    public async Task CreateIdeaAsync_WhenTopicDoesNotExist_ShouldThrowTopicNotFoundException()
    {
        // Arrange
        var topicRepositoryMock = new Mock<ITopicRepository>();
        var userRepositoryMock = new Mock<IUserRepository>();
        var ideaRepositoryMock = new Mock<IIdeaRepository>();

        topicRepositoryMock
            .Setup(r => r.ReadTopicByIdAsync(1))
            .ReturnsAsync((Topic?)null);

        var service = new IdeaCommandService(
            topicRepositoryMock.Object,
            userRepositoryMock.Object,
            ideaRepositoryMock.Object);

        // Act
        var act = async () => await service.CreateIdeaAsync(
            1, "My idea", "My description", 99);

        // Assert
        await act.Should().ThrowAsync<TopicNotFoundException>();

        userRepositoryMock.Verify(
            r => r.ExistsAsync(It.IsAny<ulong>()),
            Times.Never);

        ideaRepositoryMock.Verify(
            r => r.CreateIdeaAsync(It.IsAny<Idea>()),
            Times.Never);
    }

    [Fact]
    public async Task CreateIdeaAsync_WhenTopicIsClosed_ShouldThrowTopicClosedException()
    {
        // Arrange
        var topicRepositoryMock = new Mock<ITopicRepository>();
        var userRepositoryMock = new Mock<IUserRepository>();
        var ideaRepositoryMock = new Mock<IIdeaRepository>();

        topicRepositoryMock
            .Setup(r => r.ReadTopicByIdAsync(1))
            .ReturnsAsync(new Topic
            {
                Id = 1,
                Status = "closed"
            });

        var service = new IdeaCommandService(
            topicRepositoryMock.Object,
            userRepositoryMock.Object,
            ideaRepositoryMock.Object);

        // Act
        var act = async () => await service.CreateIdeaAsync(
            1, "My idea", "My description", 99);

        // Assert
        await act.Should().ThrowAsync<TopicClosedException>();

        userRepositoryMock.Verify(
            r => r.ExistsAsync(It.IsAny<ulong>()),
            Times.Never);

        ideaRepositoryMock.Verify(
            r => r.CreateIdeaAsync(It.IsAny<Idea>()),
            Times.Never);
    }

    [Fact]
    public async Task CreateIdeaAsync_WhenUserDoesNotExist_ShouldThrowUserNotFoundException()
    {
        // Arrange
        var topicRepositoryMock = new Mock<ITopicRepository>();
        var userRepositoryMock = new Mock<IUserRepository>();
        var ideaRepositoryMock = new Mock<IIdeaRepository>();

        topicRepositoryMock
            .Setup(r => r.ReadTopicByIdAsync(1))
            .ReturnsAsync(new Topic
            {
                Id = 1,
                Status = "open"
            });

        userRepositoryMock
            .Setup(r => r.ExistsAsync(99))
            .ReturnsAsync(false);

        var service = new IdeaCommandService(
            topicRepositoryMock.Object,
            userRepositoryMock.Object,
            ideaRepositoryMock.Object);

        // Act
        var act = async () => await service.CreateIdeaAsync(
            1, "My idea", "My description", 99);

        // Assert
        await act.Should().ThrowAsync<UserNotFoundException>();

        ideaRepositoryMock.Verify(
            r => r.CreateIdeaAsync(It.IsAny<Idea>()),
            Times.Never);
    }
}
