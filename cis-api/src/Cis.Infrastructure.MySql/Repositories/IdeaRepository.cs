using Cis.Core.Domain.Entities;
using Cis.Core.Domain.Repositories;
using Cis.Infrastructure.MySql.Context;
using Cis.Infrastructure.MySql.Mappers;
using Microsoft.EntityFrameworkCore;

namespace Cis.Infrastructure.MySql.Repositories;

public class IdeaRepository(CisDbContext context) : IIdeaRepository
{
    public async Task<Idea> CreateIdeaAsync(Idea idea)
    {
        ArgumentNullException.ThrowIfNull(idea);

        var entity = IdeaInfrastructureMapper.ToMySqlEntity(idea);

        await context.Ideas.AddAsync(entity);
        await context.SaveChangesAsync();

        var savedEntity = await context.Ideas
            .AsNoTracking()
            .Include(savedIdea => savedIdea.User)
            .Include(savedIdea => savedIdea.Votes)
            .FirstOrDefaultAsync(savedIdea => savedIdea.Id == entity.Id)
            ?? throw new InvalidOperationException(
                "The created idea could not be retrieved.");

        return IdeaInfrastructureMapper.ToDomainEntity(savedEntity);
    }
}
