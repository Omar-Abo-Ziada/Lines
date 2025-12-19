using AdminLine.Framework;
using AdminLine.Service;
using AdminLine.Services;
using Lines.Application.Interfaces;
using Lines.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register HttpContextAccessor (required for UserStateService)
builder.Services.AddHttpContextAccessor();

// Register MemoryCache (required for dashboard caching)
builder.Services.AddMemoryCache();

// Register IUserStateService (required by Repository<>)
builder.Services.AddScoped<IUserStateService, UserStateService>();

// Register IDriverConnectionService (required for online drivers count)
builder.Services.AddScoped<IDriverConnectionService, DriverConnectionService>();

// Add framework configuration (registers infrastructure services)
builder.Services.AddFrameworkConfiguration(builder.Configuration);
builder.Services.AddServiceConfiguration();

// Remove the IApplicationUserService registration from Infrastructure (requires MediatR)
// and replace it with our admin-specific implementation
var descriptor = builder.Services.FirstOrDefault(d => 
    d.ServiceType == typeof(Lines.Application.Interfaces.IApplicationUserService) &&
    d.ImplementationType == typeof(Lines.Infrastructure.Services.ApplicationUserService));
if (descriptor != null)
{
    builder.Services.Remove(descriptor);
}

// Register admin-specific IApplicationUserService (doesn't require MediatR)
builder.Services.AddScoped<Lines.Application.Interfaces.IApplicationUserService, AdminApplicationUserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
