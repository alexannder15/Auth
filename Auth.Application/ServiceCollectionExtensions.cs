using Auth.Application.Interfaces;
using Auth.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationCustomExtensions(this IServiceCollection services)
    {
        services.AddScoped<IUserContextService, UserContextService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IJwtService, JwtService>();

        return services;
    }
}
