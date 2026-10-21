using Cis.Core.Application.DependencyInjection;
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
        services.AddMySqlInfrastructure(configuration);

        return services;
    }
}
