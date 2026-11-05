using Cis.Core.Domain.Entities;
using Cis.Core.Domain.Exceptions;
using Cis.Core.Domain.Repositories;
using Cis.Infrastructure.MySql.Context;
using Cis.Infrastructure.MySql.Mappers;
using Microsoft.EntityFrameworkCore;

namespace Cis.Infrastructure.MySql.Repositories;

public class TopicRepository(CisDbContext context) : ITopicRepository
{
    public async Task<Topic> CreateTopicAsync(Topic topic)
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

    public async Task<List<Topic>> ReadTopicsAsync()
    {
        var topicEntities = await context.Topics
            .AsNoTracking()
            .Include(topic => topic.User)
            .Include(topic => topic.Ideas)
            .OrderByDescending(topic => topic.CreatedAt)
            .ToListAsync();

        return [.. topicEntities.Select(TopicEntityMapper.ToDomain)];
    }

    public async Task<Topic?> ReadTopicByIdAsync(ulong id)
    {
        var topicEntity = await context.Topics
            .AsNoTracking()
            .Include(topic => topic.User)
            .Include(topic => topic.Ideas)
            .FirstOrDefaultAsync(topic => topic.Id == id);

        return topicEntity is null
            ? null
            : TopicEntityMapper.ToDomain(topicEntity);
    }

    public async Task<Topic> UpdateTopicAsync(Topic topic)
    {
        var entity = await context.Topics
            .Include(t => t.User)
            .Include(t => t.Ideas)
            .FirstOrDefaultAsync(t => t.Id == topic.Id)
            ?? throw new TopicNotFoundException($"Topic with ID {topic.Id} was not found.");

        entity.Status = topic.Status;
        entity.UpdatedAt = topic.UpdatedAt;

        await context.SaveChangesAsync();

        return TopicEntityMapper.ToDomain(entity);
    }
}
