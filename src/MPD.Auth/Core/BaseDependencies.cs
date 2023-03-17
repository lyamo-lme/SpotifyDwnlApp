namespace MPD.Auth.Core;

public static class BaseDependencies
{
    public static IServiceCollection AddBase(this IServiceCollection builder)
    {
        builder.AddControllersWithViews(); 
        return builder;
    }
}