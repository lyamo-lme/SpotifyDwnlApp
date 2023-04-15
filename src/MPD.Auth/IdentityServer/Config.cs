using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using IdentityModel;

namespace MPD.Auth.IdentityServer;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new[] { new ApiScope("api") };

    public static IEnumerable<ApiResource> ApiResources =>
        new[]
        {
            new ApiResource("api")
            {
                Scopes = new List<string> { "api" },
                ApiSecrets = new List<Secret>
                {
                    new Secret("ScopeSecret".Sha256())
                },
                UserClaims = new List<string>
                {
                    "Id",
                    JwtClaimTypes.Email
                }
            }
        };

    public static IEnumerable<Client> Clients =>
        new[]
        {
            new Client
            {
                ClientId = "api.client",
                ClientName = "Client Credentials Client",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets =
                {
                    new Secret("ClientSecret1".Sha256())
                },
                AllowedScopes =
                {
                    "api",
                    "UserClaims",
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile
                },
                AccessTokenLifetime = 60 * 2,
                AllowOfflineAccess = true,
                AlwaysIncludeUserClaimsInIdToken = true
            },
            new Client
            {
                ClientId = "spa.client",
                ClientName = "spa lient",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                RedirectUris = new List<string>()
                {
                    "http://localhost:3000/callback",
                    "http://localhost:3000"
                },
                PostLogoutRedirectUris = new List<string>()
                {
                    "http://localhost:3000/callback",
                    "http://localhost:3000"
                },
                ClientSecrets =
                {
                    new Secret("SpaSecretKey".Sha256())
                },
                AllowedScopes =
                {
                    "api",
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile
                },
                AccessTokenLifetime = 60 * 10,
                AllowOfflineAccess = true,
                AlwaysIncludeUserClaimsInIdToken = true,
                AllowAccessTokensViaBrowser = true
            }
        };
}