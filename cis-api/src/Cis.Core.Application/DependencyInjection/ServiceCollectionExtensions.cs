using Cis.Core.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Cis.Core.Application.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<TopicQueryService>();
        services.AddScoped<ITopicCommandService, TopicCommandService>();

        return services;
    }
}
