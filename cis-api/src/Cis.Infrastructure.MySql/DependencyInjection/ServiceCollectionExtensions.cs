using Cis.Infrastructure.MySql.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Cis.Infrastructure.MySql.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMySqlInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<TopicRepository>();

        return services;
    }
}

