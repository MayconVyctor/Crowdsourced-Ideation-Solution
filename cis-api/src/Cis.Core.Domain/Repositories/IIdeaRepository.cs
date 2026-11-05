using Cis.Core.Domain.Entities;

namespace Cis.Core.Domain.Repositories;

public interface IIdeaRepository
{
    Task<Idea> CreateIdeaAsync(Idea idea);
}
