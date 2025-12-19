using AdminLine.Service.IService;
using AdminLine.Service.Service;
using Microsoft.Extensions.DependencyInjection;

namespace AdminLine.Service
{
    public static class ConfigurationExtensions
    {
        public static IServiceCollection AddServiceConfiguration(this IServiceCollection services)
        {
            services.AddTransient<ITripService, TripService>();
            
            // Register dashboard services
            services.AddScoped<IDriverService, DriverService>();
            services.AddScoped<IFinanceService, FinanceService>();
            services.AddScoped<IRegistrationService, RegistrationService>();
            services.AddScoped<IRightsService, RightsService>();
            services.AddScoped<IDashboardService, DashboardService>();
            
            // Register driver admin services
            services.AddScoped<IDriverAdminService, DriverAdminService>();
            
            return services;
        }
    }
}
