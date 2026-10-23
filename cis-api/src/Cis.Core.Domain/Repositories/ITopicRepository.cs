using Cis.Core.Domain.Entities;

namespace Cis.Core.Domain.Repositories;

public interface ITopicRepository
{
    Task<Topic>CreateTopicAsync(Topic topic);
}