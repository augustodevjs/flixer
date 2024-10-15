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
                options.AddPolicy(Policies.VideosPolicy,
                    policy => policy.RequireRealmRoles(Roles.Videos, Roles.Admin));
                
                options.AddPolicy(Policies.GenresPolicy,
                    policy => policy.RequireRealmRoles(Roles.Genres, Roles.Admin));
                
                options.AddPolicy(Policies.CategoriesPolicy,
                    policy => policy.RequireRealmRoles(Roles.Categories, Roles.Admin));
                
                options.AddPolicy(Policies.CastMembersPolicy,
                    policy => policy.RequireRole(Roles.CastMembers, Roles.Admin));
            })
            .AddKeycloakAuthorization(configuration);
        
        return services;
    }
}