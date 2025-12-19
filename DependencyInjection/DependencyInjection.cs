using Lines.Application.ServiceRegisteration;
using Lines.Infrastructure.ServicesRegisteration;
using Mapster;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;


namespace DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApplicationServices();
        services.AddInfrastructureServices(configuration);
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddAutoMapper(Assembly.GetCallingAssembly());
        TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetCallingAssembly());


        return services;
    }
}
