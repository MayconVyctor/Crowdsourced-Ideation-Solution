namespace Cis.Core.Domain.Repositories;

public interface IUserRepository
{
    Task<bool> ExistsAsync(ulong userId);
}
