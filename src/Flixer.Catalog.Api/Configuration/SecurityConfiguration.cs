using Flixer.Catalog.Api.Authorization;
using Keycloak.AuthServices.Authorization;
using Keycloak.AuthServices.Authentication;

namespace Flixer.Catalog.Api.Configuration;

public static class SecurityConfiguration
{
    public static IServiceCollection AddSecurityConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddKeycloakWebApiAuthentication(configuration);
        services
            .AddAuthorization(options =>
            {
                options.AddPolicy(Policies.VideosManager, 
                    policy => policy.RequireRealmRoles(Roles.Videos, Roles.Admin));
            })
            .AddKeycloakAuthorization(configuration);
        
        return services;
    }
}