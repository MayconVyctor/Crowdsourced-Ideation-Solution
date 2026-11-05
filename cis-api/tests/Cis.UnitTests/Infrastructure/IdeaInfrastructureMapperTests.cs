using Cis.Infrastructure.MySql.Mappers;
using FluentAssertions;
using Xunit;
using DomainIdea = Cis.Core.Domain.Entities.Idea;
using MySqlIdea = Cis.Infrastructure.MySql.Entities.Idea;

namespace Cis.UnitTests.Infrastructure;

public class IdeaInfrastructureMapperTests
{
    [Fact]
    public void ToMySqlEntity_WhenValidDomainIdea_ShouldMapToMySqlEntity()
    {
        // Arrange
        var now = DateTime.UtcNow;

        var domain = new DomainIdea
        {
            Id = 1,
            Title = "Domain Driven Design",
            Description = "An idea about DDD",
            UserId = 5,
            TopicId = 10,
            CreatedAt = now,
            UpdatedAt = now
        };

        // Act
        var entity = IdeaInfrastructureMapper.ToMySqlEntity(domain);

        // Assert
        entity.Should().NotBeNull();
        entity.Id.Should().Be(1);
        entity.Title.Should().Be("Domain Driven Design");
        entity.Description.Should().Be("An idea about DDD");
        entity.UserId.Should().Be(5);
        entity.TopicId.Should().Be(10);
        entity.CreatedAt.Should().Be(now);
        entity.UpdatedAt.Should().Be(now);
    }

    [Fact]
    public void ToDomainEntity_WhenValidMySqlEntity_ShouldMapToDomainIdea()
    {
        // Arrange
        var now = DateTime.UtcNow;

        var entity = new MySqlIdea
        {
            Id = 2,
            Title = "Microservices",
            Description = "An idea about microservices",
            UserId = 8,
            TopicId = 20,
            CreatedAt = now,
            UpdatedAt = now,
            User = new Cis.Infrastructure.MySql.Entities.User
            {
                Id = 8,
                Login = "ellen"
            },
            Votes =
            [
                new Cis.Infrastructure.MySql.Entities.Vote
                {
                    IsActive = true
                },
                new Cis.Infrastructure.MySql.Entities.Vote
                {
                    IsActive = true
                },
                new Cis.Infrastructure.MySql.Entities.Vote
                {
                    IsActive = false
                }
            ]
        };

        // Act
        var domain = IdeaInfrastructureMapper.ToDomainEntity(entity);

        // Assert
        domain.Should().NotBeNull();
        domain.Id.Should().Be(2);
        domain.Title.Should().Be("Microservices");
        domain.Description.Should().Be("An idea about microservices");
        domain.UserId.Should().Be(8);
        domain.TopicId.Should().Be(20);
        domain.Owner.Should().NotBeNull();
        domain.Owner!.Id.Should().Be(8);
        domain.Owner.Login.Should().Be("ellen");
        domain.VoteCount.Should().Be(2);
        domain.CreatedAt.Should().Be(now);
        domain.UpdatedAt.Should().Be(now);
    }

    [Fact]
    public void ToDomainEntity_WhenUserIsNotLoaded_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var entity = new MySqlIdea
        {
            Id = 1,
            Title = "Idea",
            Description = "Description",
            UserId = 5,
            TopicId = 10,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Act
        Action act = () => IdeaInfrastructureMapper.ToDomainEntity(entity);

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("The idea must have its owner loaded.");
    }

    [Fact]
    public void ToMySqlEntity_WhenDomainIdeaIsNull_ShouldThrowArgumentNullException()
    {
        // Act
        Action act = () => IdeaInfrastructureMapper.ToMySqlEntity(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ToDomainEntity_WhenMySqlIdeaIsNull_ShouldThrowArgumentNullException()
    {
        // Act
        Action act = () => IdeaInfrastructureMapper.ToDomainEntity(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }
}
