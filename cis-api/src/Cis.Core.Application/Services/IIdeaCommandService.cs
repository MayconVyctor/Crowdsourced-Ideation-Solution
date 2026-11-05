using Cis.Core.Domain.Entities;

namespace Cis.Core.Application.Services;

public interface IIdeaCommandService
{
    Task<Idea> CreateIdeaAsync(
        ulong topicId,
        string title,
        string description,
        ulong userId);
}
