using Cis.Core.Domain.Repositories;
using Cis.Infrastructure.MongoDb.Documents;
using Cis.Infrastructure.MongoDb.Mappers;
using MongoDB.Driver;
using DomainTopic = Cis.Core.Domain.Entities.Topic;

namespace Cis.Infrastructure.MongoDb.Repositories;

/// <summary>
/// Persiste tópicos no MongoDB sem implementar regras de negócio.
/// </summary>
public sealed class TopicRepository(IMongoDatabase database) : ITopicRepository
{
    private readonly IMongoCollection<TopicDocument> _topics =
        database.GetCollection<TopicDocument>("topics");

    private readonly IMongoCollection<CounterDocument> _counters =
        database.GetCollection<CounterDocument>("counters");

    public async Task<DomainTopic> CreateTopicAsync(DomainTopic topic)
    {
        ArgumentNullException.ThrowIfNull(topic);

        var counter = await _counters.FindOneAndUpdateAsync(
            Builders<CounterDocument>.Filter.Eq(counter => counter.Id, "topics"),
            Builders<CounterDocument>.Update.Inc(
                counter => counter.SequenceValue, 1),
            new FindOneAndUpdateOptions<CounterDocument>
            {
                IsUpsert = true,
                ReturnDocument = ReturnDocument.After
            });

        var document = TopicMapper.ToDocument(topic);
        document.Id = counter.SequenceValue;

        await _topics.InsertOneAsync(document);

        return TopicMapper.ToDomain(document);
    }

    public async Task<List<DomainTopic>> ReadTopicsAsync()
    {
        var documents = await _topics
            .Find(Builders<TopicDocument>.Filter.Empty)
            .SortByDescending(topic => topic.CreatedAt)
            .ToListAsync();

        return [.. documents.Select(TopicMapper.ToDomain)];
    }

    public async Task<DomainTopic?> ReadTopicByIdAsync(ulong id)
    {
        if (id > long.MaxValue)
            return null;

        var document = await _topics
            .Find(topic => topic.Id == (long)id)
            .FirstOrDefaultAsync();

        return document is null
            ? null
            : TopicMapper.ToDomain(document);
    }

    public async Task<DomainTopic> UpdateTopicAsync(DomainTopic topic)
    {
        ArgumentNullException.ThrowIfNull(topic);

        var filter = Builders<TopicDocument>.Filter.Eq(t => t.Id, (long)topic.Id);
        var update = Builders<TopicDocument>.Update
            .Set(t => t.Status, topic.Status)
            .Set(t => t.UpdatedAt, topic.UpdatedAt);

        await _topics.UpdateOneAsync(filter, update);

        return topic;
    }
}
