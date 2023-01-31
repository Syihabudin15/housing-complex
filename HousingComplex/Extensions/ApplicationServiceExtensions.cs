using System.Text;
using HousingComplex.Middleware;
using HousingComplex.Repositories;
using HousingComplex.Security;
using HousingComplex.Services;
using HousingComplex.Services.Impl;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace HousingComplex.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(optionsBuilder =>
        {
            optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));
        });
        services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
        services.AddTransient<IPersistence, DbPersistence>();
        services.AddTransient<IRoleService, RoleService>();
        services.AddTransient<IAuthService, AuthService>();
        services.AddTransient<ICustomerService, CustomerService>();
        services.AddTransient<IJwtUtils, JwtUtils>();
        services.AddTransient<IDeveloperService, DeveloperService>();

        services.AddScoped<ExceptionHandlingMiddleware>();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                ValidIssuer = config["JwtSettings:Issuer"],
                ValidAudience = config["JwtSettings:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtSettings:Key"]))
            };
        });

        return services;
    }
}