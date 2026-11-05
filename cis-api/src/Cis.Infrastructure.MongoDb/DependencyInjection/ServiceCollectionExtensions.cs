using Cis.Core.Domain.Repositories;
using Cis.Infrastructure.MongoDb.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Cis.Infrastructure.MongoDb.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMongoDbInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString =
            configuration.GetConnectionString("CisMongo");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "Connection string 'CisMongo' was not configured.");
        }

        var mongoUrl = MongoUrl.Create(connectionString);

        if (string.IsNullOrWhiteSpace(mongoUrl.DatabaseName))
        {
            throw new InvalidOperationException(
                "Connection string 'CisMongo' must include the database name.");
        }

        services.AddSingleton<IMongoClient>(
            _ => new MongoClient(connectionString));

        services.AddSingleton<IMongoDatabase>(serviceProvider =>
            serviceProvider
                .GetRequiredService<IMongoClient>()
                .GetDatabase(mongoUrl.DatabaseName));

        services.AddScoped<ITopicRepository, TopicRepository>();

        return services;
    }
}
