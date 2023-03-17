using MPD.Auth.Models.Config;

namespace MPD.Auth.IdentityServer.ExternalProviders;

public static class SpotifyExternalProvider
{
    public static IServiceCollection AddSpotifyAuth(this IServiceCollection builder, ServiceSetting model)
    {
        builder.AddAuthentication().AddSpotify(conf =>
        {
            conf.ClientId = model.ClientId;
            conf.ClientSecret = model.ClientSecret;
        });
        return builder;
    }
}