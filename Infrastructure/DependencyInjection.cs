using Infrastructure.Context;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Mapster;
using System.Reflection;
using MapsterMapper;
using Core.Request;
using Infrastructure.Validations;
using FluentValidation;
using Infrastructure.Repositories;
using Core.Interfaces.Repository;
using Core.Interfaces.Service;
using Infrastructure.Services;
using Core.Jwt;
using Infrastructure.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Core.Interfaces.Auth;
using Core.DTOs.User;
using Core.DTOs.CategoryExpense;
using Core.DTOs.Expense;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
        )
    {
        services.AddAuth(configuration);
        services.AddDatabase(configuration);
        services.AddRepositories();
        services.AddServices();
        services.AddValidations();
        services.AddMapping();

        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services,
        IConfiguration configuration
        )
    {
        var connectionString = configuration.GetConnectionString("DbConnection");

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection service)
    {
        service.AddScoped<IUserRepository, UserRepository>();
        service.AddScoped<ICategoryExpenseRepository, CategoryExpenseRepository>();
        service.AddScoped<IExpenseRepository, ExpenseRepository>();
        service.AddScoped<IJwtProvider, JwtProvider>();

        return service;
    }

    private static IServiceCollection AddServices(this IServiceCollection service)
    {
        service.AddScoped<IUserService, UserService>();
        service.AddScoped<ICategoryExpenseService, CategoryExpenseService>();
        service.AddScoped<IExpenseService, ExpenseService>();

        return service;
    }

    private static IServiceCollection AddValidations(this IServiceCollection service)
    {
        service.AddScoped<IValidator<CreateUserRequest>, CreateUserRequestValidator>();
        service.AddScoped<IValidator<UpdateUserDto>, UpdateUserValidator>();
        service.AddScoped<IValidator<CreateCategoryExpenseRequest>, CreateCategoryExpenseValidator>();
        service.AddScoped<IValidator<UpdateCategoryExpenseDto>, UpdateCategoryExpenseValidator>();
        service.AddScoped<IValidator<CreateExpenseRequest>, CreateExpenseValidator>();
        service.AddScoped<IValidator<UpdateExpenseDto>, UpdateExpenseValidator>();

        return service;
    }

    private static IServiceCollection AddMapping(this IServiceCollection service)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(Assembly.GetExecutingAssembly());

        service.AddSingleton(config);
        service.AddScoped<IMapper, ServiceMapper>();

        return service;
    }

    private static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        configuration.GetSection("JWT").Get<JwtOptions>();

        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = configuration["JWT:Issuer"],
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                RequireExpirationTime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]!))
            };
        });

        services.AddTransient<JwtProvider>();

        return services;
    }
}
