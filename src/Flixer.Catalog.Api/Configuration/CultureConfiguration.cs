using System.Globalization;
using Microsoft.AspNetCore.Localization;

namespace Flixer.Catalog.Api.Configuration;

public static class CultureConfiguration
{
    public static IServiceCollection ConfigureCulture(this IServiceCollection services)
    {
        services
            .Configure<RequestLocalizationOptions>(o => 
            {
                var supportedCultures = new[] { new CultureInfo("pt-BR") };
                o.DefaultRequestCulture = new RequestCulture("pt-BR", "pt-BR");
                o.SupportedCultures = supportedCultures;
                o.SupportedUICultures = supportedCultures;
            });
        
        return services;
    }
    
    public static IApplicationBuilder UseConfiguredRequestLocalization(this IApplicationBuilder app)
    {
        var supportedCultures = new[] { new CultureInfo("pt-BR") };
            
        app.UseRequestLocalization(new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture(culture: "pt-BR", uiCulture: "pt-BR"),
            SupportedCultures = supportedCultures,
            SupportedUICultures = supportedCultures
        });

        return app;
    }
}