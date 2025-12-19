using Lines.Application.Features.FCMNotifications.Queries;
using Lines.Application.Interfaces;
using Lines.Application.Interfaces.Notifications;
using Lines.Application.Services;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
 

namespace Lines.Application.ServiceRegisteration;

public static class ApplicationServiceRegisteration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        //services.AddMediatR(Assembly.GetExecutingAssembly());
        //services.AddMediatR(cfg =>
        // cfg.RegisterServicesFromAssembly(typeof(ApplicationServiceRegisteration).Assembly));

        services.AddMediatR(Assembly.GetExecutingAssembly());


        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());

        // Register application services
        services.AddScoped<IServiceFeeService, ServiceFeeService>();

        return services;
    }
}
