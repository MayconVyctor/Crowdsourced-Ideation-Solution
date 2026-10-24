using Cis.Infrastructure.MySql.Mappers;
using FluentAssertions;
using Xunit;
using DomainTopic = Cis.Core.Domain.Entities.Topic;
using MySqlTopic = Cis.Infrastructure.MySql.Entities.Topic;

namespace Cis.UnitTests.Infrastructure;

public class TopicInfrastructureMapperTests
{
    [Fact]
    public void ToMySqlEntity_WhenValidDomainTopic_ShouldMapToMySqlEntity()
    {
        var now = DateTime.UtcNow;
        var domain = new DomainTopic
        {
            Id = 1,
            Title = "Domain Driven Design",
            Description = "Conceitos de DDD",
            Status = "open",
            UserId = 5,
            CreatedAt = now,
            UpdatedAt = now
        };

        var entity = TopicInfrastructureMapper.ToMySqlEntity(domain);

        entity.Should().NotBeNull();
        entity.Id.Should().Be(1);
        entity.Title.Should().Be("Domain Driven Design");
        entity.Description.Should().Be("Conceitos de DDD");
        entity.Status.Should().Be("open");
        entity.UserId.Should().Be(5);
        entity.CreatedAt.Should().Be(now);
        entity.UpdatedAt.Should().Be(now);
    }

    [Fact]
    public void ToDomainEntity_WhenValidMySqlEntity_ShouldMapToDomainTopic()
    {

        var now = DateTime.UtcNow;
        var entity = new MySqlTopic
        {
            Id = 2,
            Title = "Micronaut vs Quarkus",
            Description = "Comparativo de frameworks",
            Status = "open",
            UserId = 8,
            CreatedAt = now,
            UpdatedAt = now
        };

        var domain = TopicInfrastructureMapper.ToDomainEntity(entity);

        domain.Should().NotBeNull();
        domain.Id.Should().Be(2);
        domain.Title.Should().Be("Micronaut vs Quarkus");
        domain.Description.Should().Be("Comparativo de frameworks");
        domain.Status.Should().Be("open");
        domain.UserId.Should().Be(8);
        domain.IdeaCount.Should().Be(0);
        domain.CreatedAt.Should().Be(now);
        domain.UpdatedAt.Should().Be(now);
    }
}