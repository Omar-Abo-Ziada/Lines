using AdminLine.Framework.UoW;
using Lines.Infrastructure.ServicesRegisteration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AdminLine.Framework
{
    public static class ConfigurationExtensions
    {
        public static IServiceCollection AddFrameworkConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddHttpContextAccessor();
            services.AddInfrastructureServices(configuration);

            return services;
        }
    }
}
