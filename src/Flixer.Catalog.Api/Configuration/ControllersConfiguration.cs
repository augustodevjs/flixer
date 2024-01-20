using Flixer.Catalog.Api.Filters;

namespace Flixer.Catalog.Api.Configuration;

public static class ControllersConfiguration
{
    public static void AddAndConfigureControllers(this IServiceCollection services)
    {
        services.AddControllers(options => options.Filters.Add(typeof(ApiExceptionFilter)));
        services.AddDocumentation();
    }

    private static void AddDocumentation(this IServiceCollection services)
    {
        services.AddSwaggerGen();
        services.AddEndpointsApiExplorer();
    }
}
