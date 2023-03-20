using Microsoft.AspNetCore.Identity;
using MPD.Auth.IdentityServer.ExternalProviders;
using MPD.Core;
using MPD.Core.Data;
using MPD.Core.Entities;
using MPD.Core.ExternalProviderServices;
using MPD.Data.DbContext;
using MPD.Data.Repository;
using MPD.Services.ExternalProviders;

namespace MPD.Auth.Core;

public static class BaseDependencies
{
    public static IServiceCollection AddBase(this IServiceCollection builder)
    {
        
        builder.AddIdentityCore<User>(o =>
            {
                o.Stores.MaxLengthForKeys = 128;
            })
            .AddDefaultUI()
            .AddEntityFrameworkStores<AppDbContext>();
        
        builder.ConfigureApplicationCookie(config =>
        {
            config.Cookie.Name = "IS.Cookie";
            config.LoginPath = "/Auth/Login";
            config.LogoutPath = "/Auth/Logout";
        });

        builder.AddTransient<ISpotifyProviderService, SpotifyProviderService>();
        builder.AddTransient<IUnitOfWorkRepository, UowRepository>();
        builder.AddTransient<IRepositoryFactory, RepositoryFactory>();
        
        builder.AddControllersWithViews(); 
        return builder;
    }
}