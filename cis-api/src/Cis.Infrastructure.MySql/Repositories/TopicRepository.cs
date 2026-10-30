using Cis.Core.Domain.Exceptions;
using Cis.Core.Domain.Repositories;
using Cis.Infrastructure.MySql.Context;
using Cis.Infrastructure.MySql.Mappers;
using Microsoft.EntityFrameworkCore;
using DomainTopic = Cis.Core.Domain.Entities.Topic;

namespace Cis.Infrastructure.MySql.Repositories;

public class TopicRepository(CisDbContext context) : ITopicRepository
{
    public async Task<DomainTopic> CreateTopicAsync(DomainTopic topic)
    {
        var userExists = await context.Users.AnyAsync(u => u.Id == topic.UserId);
        if (!userExists)
        {
            throw new CisUnprocessableEntityException($"User with id {topic.UserId} does not exist.");
        }

        var mySqlEntity = TopicInfrastructureMapper.ToMySqlEntity(topic);
        await context.Topics.AddAsync(mySqlEntity);
        await context.SaveChangesAsync();
        return TopicInfrastructureMapper.ToDomainEntity(mySqlEntity);
    }
}
