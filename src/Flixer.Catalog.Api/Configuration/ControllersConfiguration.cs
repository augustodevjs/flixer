namespace Flixer.Catalog.Api.Configuration;

public static class ControllersConfiguration
{
    public static void AddAndConfigureControllers(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddDocumentation();
    }

    private static void AddDocumentation(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }
}
