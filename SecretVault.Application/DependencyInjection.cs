using SecretVault.Application.Services;
using SecretVault.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace SecretVault.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ISecretService, SecretService>();
        services.AddScoped<ISharingService, SharingService>();
        return services;
    }
}