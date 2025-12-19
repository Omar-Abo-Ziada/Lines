using DependencyInjection;
using Lines.Application.Common;
using Lines.Application.Interfaces;
using Lines.Infrastructure.Services;
using Lines.Presentation.Firebase;
using Lines.Presentation.Middlewares;
using Lines.Presentation.Services;
using Microsoft.OpenApi.Models;
using Serilog;
namespace Lines.Presentation;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        //var config = builder.Configuration;
        //var environment = builder.Environment;
        //builder.Services.AddAuthentication(options =>
        //{
        //    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        //    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        //})

        // .AddApple(options =>
        //    {
        //    options.ClientId = config["Authentication:Apple:ClientId"];
        //    options.KeyId = config["Authentication:Apple:KeyId"];
        //    options.TeamId = config["Authentication:Apple:TeamId"];

        //        options.UsePrivateKey(keyId =>
        //   environment.ContentRootFileProvider
        //              .GetFileInfo($"AuthKeys/AuthKey_{keyId}.p8"));
        //    });

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        //builder.Services.AddSwaggerGen();
        builder.Services.AddValidatorsFromAssemblyContaining<Program>(); // Scans current assembly
        builder.Services.AddScoped<IUserStateService, UserStateService>();
        builder.Services.AddScoped<IDriverConnectionService, DriverConnectionService>();
        builder.Services.AddTransient<ExceptionHandlingMiddleware>();
        builder.Services.AddScoped(typeof(BaseControllerParams<>));
        builder.Services.AddScoped(typeof(RequestHandlerBaseParameters));
        builder.Services.AddAutoMapper(typeof(Program).Assembly);
        builder.Services.AddMemoryCache();
        builder.Services.AddSignalR();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.WithOrigins
                ("http://localhost:5084",
                    "https://localhost:5083",
                    "https://localhost:7292",
                    "http://127.0.0.1:5500")
                             .AllowAnyMethod()
                             .AllowAnyHeader()
                             .AllowCredentials();
            });
        });


        builder.Services.RegisterServices(builder.Configuration);
        builder.Services.AddFirebaseAdmin(builder.Configuration);

        // Enable Swagger
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(swagger =>
        {
            swagger.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Lines",
                Description = "ASP .NET Core 8 Web API",
            });

            // Add security definition for JWT
            swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
            });

            swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
        });


        // ✅ Read Serilog config from appsettings
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger();


        builder.Host.UseSerilog();

        #region stripe 

        //// ربط إعدادات Stripe من appsettings
        //builder.Services.Configure<StripeOptions>(
        //    builder.Configuration.GetSection("Stripe"));

        //// تسجيل الـ gateway
        //builder.Services.AddScoped<IPaymentGateway, StripePaymentGateway>();

        #endregion

        var app = builder.Build();
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.UseMiddleware<TransactionMiddleware>();
        // Configure the HTTP request pipeline.
        // if (!app.Environment.IsDevelopment())
        // {
        app.UseSwagger();
        app.UseSwaggerUI();
        // }

        //app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseCors("AllowAll");
        app.UseAuthentication();
        app.UseAuthorization();

        //app.UseMiddleware<TransactionMiddleware>();
        app.MapControllers();
        app.MapHub<DriverHub>("/driverHub");
        app.MapHub<ChatHub>("/chatHub");
        app.Run();
    }
}
