using MongoDB.Bson;
using Cis.Infrastructure.MongoDb.Documents;
using Cis.Infrastructure.MongoDb.Mappers;
using FluentAssertions;
using Xunit;
using DomainTopic = Cis.Core.Domain.Entities.Topic;

namespace Cis.UnitTests.Infrastructure;

public class TopicMongoMapperTests
{
    [Fact]
    public void ToDocument_WhenValidDomainTopic_ShouldMapAllFields()
    {
        var now = DateTime.UtcNow;
        var domain = new DomainTopic
        {
            Id = 1,
            Title = "Domain Driven Design",
            Description = "Conceitos de DDD",
            Status = "open",
            UserId = 5,
            IdeaCount = 3,
            CreatedAt = now,
            UpdatedAt = now
        };

        var document = TopicMapper.ToDocument(domain);

        document.Id.Should().Be(1);
        document.Title.Should().Be("Domain Driven Design");
        document.Description.Should().Be("Conceitos de DDD");
        document.Status.Should().Be("open");
        document.UserId.Should().Be(5);
        document.IdeaCount.Should().Be(3);
        document.CreatedAt.Should().Be(now);
        document.UpdatedAt.Should().Be(now);
        document.SchemaVersion.Should().Be(TopicDocument.CurrentSchemaVersion);
    }

    [Fact]
    public void ToDomain_WhenValidDocument_ShouldMapAllDomainFields()
    {
        var now = DateTime.UtcNow;
        var document = new TopicDocument
        {
            Id = 2,
            Title = "Micronaut vs Quarkus",
            Description = "Comparativo de frameworks",
            Status = "open",
            UserId = 8,
            IdeaCount = 4,
            CreatedAt = now,
            UpdatedAt = now,
            SchemaVersion = 1
        };

        var domain = TopicMapper.ToDomain(document);

        domain.Id.Should().Be(2);
        domain.Title.Should().Be("Micronaut vs Quarkus");
        domain.Description.Should().Be("Comparativo de frameworks");
        domain.Status.Should().Be("open");
        domain.UserId.Should().Be(8);
        domain.IdeaCount.Should().Be(4);
        domain.CreatedAt.Should().Be(now);
        domain.UpdatedAt.Should().Be(now);
    }

    [Fact]
    public void ToDocument_ShouldUseMongoSpecificFieldNames()
    {
        var document = new TopicDocument
        {
            Id = 1,
            Title = "Test",
            UserId = 5,
            IdeaCount = 2
        };

        var bson = document.ToBsonDocument();

        bson.Contains("_id").Should().BeTrue();
        bson.Contains("title").Should().BeTrue();
        bson.Contains("user_id").Should().BeTrue();
        bson.Contains("idea_count").Should().BeTrue();
        bson.Contains("schema_version").Should().BeTrue();

        bson.Contains("Id").Should().BeFalse();
        bson.Contains("UserId").Should().BeFalse();
        bson["schema_version"].AsInt32.Should()
            .Be(TopicDocument.CurrentSchemaVersion);
    }

    [Fact]
    public void ToDocument_WhenDomainIdExceedsLongRange_ShouldThrow()
    {
        var domain = new DomainTopic
        {
            Id = ulong.MaxValue,
            UserId = 1
        };

        var act = () => TopicMapper.ToDocument(domain);

        act.Should().Throw<OverflowException>();
    }

    [Fact]
    public void ToDomain_WhenDocumentIdIsNegative_ShouldThrow()
    {
        var document = new TopicDocument { Id = -1 };

        var act = () => TopicMapper.ToDomain(document);

        act.Should().Throw<OverflowException>();
    }

    [Fact]
    public void ToDocument_WhenDomainIsNull_ShouldThrow()
    {
        var act = () => TopicMapper.ToDocument(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ToDomain_WhenDocumentIsNull_ShouldThrow()
    {
        var act = () => TopicMapper.ToDomain(null!);

        act.Should().Throw<ArgumentNullException>();
    }
}
