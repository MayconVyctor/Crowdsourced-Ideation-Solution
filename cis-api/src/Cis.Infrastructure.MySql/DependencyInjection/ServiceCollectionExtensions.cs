using Cis.Core.Domain.Repositories;
using Cis.Infrastructure.MySql.Context;
using Cis.Infrastructure.MySql.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cis.Infrastructure.MySql.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMySqlInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString =
            configuration.GetConnectionString("CisDatabase");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "Connection string 'CisDatabase' was not configured.");
        }

        services.AddDbContext<CisDbContext>(options =>
            options.UseMySQL(connectionString));

        services.AddScoped<ITopicRepository, TopicRepository>();
        services.AddScoped<TopicRepository>();

        return services;
    }
}
