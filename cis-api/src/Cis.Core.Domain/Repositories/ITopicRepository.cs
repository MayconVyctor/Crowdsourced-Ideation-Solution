using Cis.Core.Domain.Entities;

namespace Cis.Core.Domain.Repositories;

public interface ITopicRepository
{
    Task<Topic> CreateTopicAsync(Topic topic);
    Task<List<Topic>> ReadTopicsAsync();
    Task<Topic?> ReadTopicByIdAsync(ulong id);
    Task<Topic> UpdateTopicAsync(Topic topic);
}
