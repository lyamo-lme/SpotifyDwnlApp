using MPD.Auth.IdentityServer;
using MPD.Core;

namespace MPD.Auth;

public static class ISDIExtension
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