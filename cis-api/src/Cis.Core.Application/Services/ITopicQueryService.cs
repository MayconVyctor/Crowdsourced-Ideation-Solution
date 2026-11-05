using Cis.Core.Domain.Entities;

namespace Cis.Core.Application.Services;

public interface ITopicQueryService
{
    Task<List<Topic>> ReadTopicsAsync();
}
