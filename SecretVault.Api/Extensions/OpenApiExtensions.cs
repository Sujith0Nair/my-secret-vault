using Scalar.AspNetCore;
using Microsoft.OpenApi.Models;

namespace SecretVault.Api.Extensions;

public static class OpenApiExtensions
{
    public static IServiceCollection AddSwaggerAndScalar(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer((document, context, cancellationToken) =>
            {
                document.Components ??= new OpenApiComponents();
                document.Components.SecuritySchemes.Add("Bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Description = "Enter your JWT token. You do NOT need to type 'Bearer ' - just the token."
                });

                document.SecurityRequirements.Add(new OpenApiSecurityRequirement
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

                return Task.CompletedTask;
            });
        });

        return services;
    }
    
    public static IApplicationBuilder UseScalar(this IApplicationBuilder app)
    {
        if (app is not WebApplication webApp || !webApp.Environment.IsDevelopment()) return app;
        webApp.MapOpenApi();
        webApp.MapScalarApiReference(options => 
        {
            options.WithTitle("My Secret Vault API")
                .WithTheme(ScalarTheme.Moon)
                .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
        });

        return app;
    }

    public static void SetupScalarUi(this WebApplication app)
    {
        app.MapOpenApi();
        app.MapScalarApiReference(options =>
        {
            options.WithTitle("SecretVault API")
                .WithTheme(ScalarTheme.Moon)
                .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
        });
    }
}