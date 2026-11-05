using Cis.Core.Domain.Repositories;
using Cis.Infrastructure.MySql.Context;
using Microsoft.EntityFrameworkCore;

namespace Cis.Infrastructure.MySql.Repositories;

public class UserRepository(CisDbContext context) : IUserRepository
{
    public async Task<bool> ExistsAsync(ulong userId)
    {
        return await context.Users
            .AnyAsync(user => user.Id == userId);
    }
}
