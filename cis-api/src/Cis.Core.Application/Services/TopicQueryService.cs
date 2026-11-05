using Cis.Core.Domain.Entities;
using Cis.Core.Domain.Repositories;

namespace Cis.Core.Application.Services;

public class TopicQueryService(ITopicRepository topicRepository)
    : ITopicQueryService
{
    public async Task<List<Topic>> ReadTopicsAsync()
    {
        var topics = await topicRepository.ReadTopicsAsync();

        return [.. topics.OrderByDescending(topic => topic.CreatedAt)];
    }
}
