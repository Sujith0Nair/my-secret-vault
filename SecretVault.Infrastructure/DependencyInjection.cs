using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using SecretVault.Application.Interfaces;
using SecretVault.Infrastructure.Services;
using SecretVault.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace SecretVault.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<SecretVaultDbContext>(options => options.UseSqlServer(connectionString));

        services.AddIdentityCore<ApplicationUser>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 8;
            options.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<SecretVaultDbContext>()
        .AddDefaultTokenProviders();
        
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IEncryptionService, EncryptionService>();
        
        return services;
    }
}