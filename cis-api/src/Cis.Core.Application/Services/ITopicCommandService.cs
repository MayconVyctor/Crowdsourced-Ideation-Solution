using Cis.Core.Domain.Entities;

namespace Cis.Core.Application.Services;

public interface ITopicCommandService
{
    Task<Topic> CreateTopicAsync(string title, string description, ulong userId);
}