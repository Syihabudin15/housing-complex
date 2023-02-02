using System.Text;
using HousingComplex.Middleware;
using HousingComplex.Repositories;
using HousingComplex.Security;
using HousingComplex.Services;
using HousingComplex.Services.Impl;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
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
        services.AddTransient<IHousingService, HousingService>();
        services.AddTransient<IFileService, FileService>();
        services.AddTransient<IImageHouseTypeService, ImageHouseTypeService>();
        services.AddTransient<ISpesificationService, SpesificationService>();
        services.AddTransient<IHouseTypeService, HouseTypeService>();
        services.AddTransient<IMeetService, MeetService>();
        services.AddTransient<ITransactionService, TransactionService>();

        services.Configure<KestrelServerOptions>(options =>
        {
            options.Limits.MaxRequestBodySize = int.MaxValue;
        });
        services.Configure<FormOptions>(options =>
        {
            options.ValueLengthLimit = int.MaxValue;
            options.MultipartBodyLengthLimit = int.MaxValue;
            options.MultipartHeadersLengthLimit = int.MaxValue;
        });

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