using Serilog;
using Serilog.Events;

namespace Flixer.Catalog.Api.Configuration;

public static class LoggingConfiguration
{
    public static void ConfigureApplicationLogging(this IHostBuilder hostBuilder)
    {
        hostBuilder.UseSerilog((_, config) =>
        {
            config.MinimumLevel.Override("Default", LogEventLevel.Information);
            config.MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning);
            config.MinimumLevel.Override("Microsoft.AspNetCore.DataProtection", LogEventLevel.Information);

            config
                .WriteTo.Console()
                .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day);
        });
    }
    
    public static void ConfigureRequestLogging(this IApplicationBuilder app)
    {
        app.UseSerilogRequestLogging(o =>
        {
            o.IncludeQueryInRequestPath = true;
        });
    }
}