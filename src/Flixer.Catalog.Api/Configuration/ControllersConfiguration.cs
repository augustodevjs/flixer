using Flixer.Catalog.Api.Filters;

namespace Flixer.Catalog.Api.Configuration;

public static class ControllersConfiguration
{
    public static IServiceCollection AddAndConfigureControllers(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("*",
                policy =>
                {
                    policy.AllowAnyHeader();
                    policy.AllowAnyMethod();
                    policy.AllowAnyOrigin();
                });
        });
        
        services.AddDocumentation();
        services.AddControllers(options => options.Filters.Add(typeof(ApiExceptionFilter)));

        return services;
    }

    private static void AddDocumentation(this IServiceCollection services)
    {
        services.AddSwaggerGen();
        services.AddEndpointsApiExplorer();
    }
    
    public static WebApplication UseDocumentation(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment()) return app;
        
        app.UseSwagger();
        app.UseSwaggerUI();

        return app;
    }
}