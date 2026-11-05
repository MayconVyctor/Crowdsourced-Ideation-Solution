using Cis.Core.Application.DependencyInjection;
using Cis.Infrastructure.MongoDb.DependencyInjection;
using Cis.Infrastructure.MySql.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cis.Infrastructure.Ioc.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCisInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddApplication();

        var provider = configuration["Persistence:Provider"] ?? "MySql";

        switch (provider.Trim().ToLowerInvariant())
        {
            case "mysql":
                services.AddMySqlInfrastructure(configuration);
                break;

            case "mongodb":
            case "mongo":
                services.AddMongoDbInfrastructure(configuration);
                break;

            default:
                throw new InvalidOperationException(
                    $"Persistence provider '{provider}' is not supported. " +
                    "Use 'MySql' or 'MongoDb'.");
        }

        return services;
    }
}
