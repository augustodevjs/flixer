using Keycloak.AuthServices.Authorization;
using Keycloak.AuthServices.Authentication;

namespace Flixer.Catalog.Api.Configuration;

public static class SecurityConfiguration
{
    public static IServiceCollection AddSecurityConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddKeycloakWebApiAuthentication(configuration);
        services.AddKeycloakAuthorization(configuration);
        
        return services;
    }
}