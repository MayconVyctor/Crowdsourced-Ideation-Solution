using Cis.Core.Domain.Repositories;
using Cis.Infrastructure.MySql.Context;
using Cis.Infrastructure.MySql.Mappers;
using DomainTopic = Cis.Core.Domain.Entities.Topic;

namespace Cis.Infrastructure.MySql.Repositories;

public class TopicRepository(CisDbContext context) : ITopicRepository
{
    public async Task<DomainTopic> CreateTopicAsync(DomainTopic topic)
    {
        var mySqlEntity = TopicInfrastructureMapper.ToMySqlEntity(topic);
        await context.Topics.AddAsync(mySqlEntity);
        await context.SaveChangesAsync();
        return TopicInfrastructureMapper.ToDomainEntity(mySqlEntity);
    }
}
