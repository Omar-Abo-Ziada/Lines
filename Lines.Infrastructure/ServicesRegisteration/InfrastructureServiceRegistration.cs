using Lines.Application.Interfaces;
using Lines.Application.Interfaces.Notifications;
using Lines.Application.Interfaces.Stripe;
using Lines.Infrastructure.Configurations;
using Lines.Infrastructure.Context;
using Lines.Infrastructure.Identity;
using Lines.Infrastructure.Repositories;
using Lines.Infrastructure.Services;
using Lines.Infrastructure.Services.Notifications;
using Lines.Infrastructure.Services.Stripe;
using Lines.Infrastructure.Settings;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

namespace Lines.Infrastructure.ServicesRegisteration;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // loading of data from appsettings.json into the MailSetting configuration class
        services.Configure<MailSettings>(configuration.GetSection("MailSettings"));

        var connectionString = configuration.GetConnectionString("DefaultConnectionString");
        services.AddAutoMapper(Assembly.GetCallingAssembly());

        services.AddDbContext<ApplicationDBContext>(options =>
            options.UseSqlServer(connectionString)
            .LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information)
            .EnableSensitiveDataLogging());


        services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<ApplicationDBContext>()
            .AddDefaultTokenProviders();

        //services.AddAuthentication(options =>
        //{
        //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        //})
        //.AddJwtBearer(options =>
        //{
        //    options.SaveToken = true;
        //    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        //    {
        //        ValidateIssuer = true,
        //        ValidateAudience = true,
        //        ValidateIssuerSigningKey = true,
        //        ValidIssuer = configuration["Jwt:Issuer"], // in case of 1 issuer
        //        ValidAudiences = configuration.GetSection("Jwt:Audience").Get<string[]>(), // Get array of audiences
        //        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
        //    };
        //});

        var googleKeys = configuration.GetRequiredSection("GoogleKeys").Get<GoogleKeys>();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)
                )
            };

            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Query["access_token"];
                    var path = context.HttpContext.Request.Path;

                    if (!string.IsNullOrEmpty(accessToken) &&
                        path.StartsWithSegments("/chatHub"))
                    {
                        context.Token = accessToken;
                    }

                    return Task.CompletedTask;
                }
            };
        })
        .AddGoogle(options =>
        {
            var googleKeys = configuration.GetSection("GoogleKeys").Get<GoogleKeys>()!;
            options.ClientId = googleKeys.Client_Id;
            options.ClientSecret = googleKeys.Client_Secret;
        });


        // Add Google Authentication
        //builder.Services.AddAuthentication(options =>
        //{
        //    options.DefaultAuthenticateScheme = GoogleDefaults.AuthenticationScheme;
        //    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
        //})
        //.AddGoogle(options =>
        //{
        //    options.ClientId = googleKeys!.Client_Id;
        //    options.ClientSecret = googleKeys.Client_Secret;
        //    options.SaveTokens = true;
        //    options.Events = new Microsoft.AspNetCore.Authentication.OAuth.OAuthEvents
        //    {
        //        OnRemoteFailure = context =>
        //        {
        //            context.HandleResponse();
        //            context.Response.Redirect("/swagger");
        //            return Task.CompletedTask;
        //        }
        //    };
        //});


        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IApplicationUserService, ApplicationUserService>();
        //services.AddScoped<IPaymentGetewayService, PaymentGetewayService>();
        services.AddTransient<IEmailService, EmailService>();
        services.AddTransient<ISmsService, TwilioService>();
        services.AddScoped<IOtpService, OtpService>();
        services.Configure<TwilioSettings>(configuration.GetSection("Twilio"));
        services.AddScoped<IApplicationDBContext>(provider =>
            provider.GetRequiredService<ApplicationDBContext>());

        // Register Background Services
        services.AddHostedService<OfferExpiryBackgroundService>();
        services.AddScoped<IFCMUserTokenService, FCMUserTokenService>();
        services.AddScoped<IFcmNotifier, FcmNotifier>();
        services.AddScoped<ISendFcmTokensQuery, SendFcmTokensQuery>();


        #region stripe 

        // ربط إعدادات Stripe من appsettings
       services.Configure<StripeOptions>(
            configuration.GetSection("Stripe"));
        // تسجيل الـ gateway
        services.AddScoped<IPaymentGateway, StripePaymentGateway>();

        #endregion

        return services;
    }
}
