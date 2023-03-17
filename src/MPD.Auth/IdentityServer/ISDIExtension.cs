using MPD.Core;

namespace MPD.Auth.IdentityServer;

public static class IdentityServerExtensions
{
    public static IServiceCollection AddIdentityServerService(this IServiceCollection builder)
    {
        
        builder.AddIdentityServer()
            .AddInMemoryClients(Config.Clients)
            .AddInMemoryApiResources(Config.ApiResources)
            .AddInMemoryApiScopes(Config.ApiScopes)
            .AddInMemoryIdentityResources(Config.IdentityResources)
            .AddAspNetIdentity<User>()
            .AddDeveloperSigningCredential();
        return builder;
    }
}