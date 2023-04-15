using Duende.IdentityServer.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MPD.Auth.Core;
using MPD.Auth.IdentityServer;
using MPD.Auth.IdentityServer.ExternalProviders;
using MPD.Auth.Models.Config;
using MPD.Core;
using MPD.Core.Entities;
using MPD.Data.DbContext;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("MsSqlConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services
    .AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(connectionString));

builder.Services
    .AddIdentity<User, IdentityRole<int>>(options =>
    {
        options.User.AllowedUserNameCharacters = string.Empty;
        options.SignIn.RequireConfirmedEmail = false;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddBase();

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("Secrets.json", optional: true);

builder.Services.AddIdentityServerService();

builder.Services.AddCors((options) =>
{
    options.AddPolicy(options.DefaultPolicyName,
        policyBuilder =>
            policyBuilder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
});

builder.Services.AddSpotifyAuth(
    builder.Configuration
        .GetSection("spotify")
        .Get<ServiceSetting>()
    ?? throw new InvalidOperationException());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCors("__DefaultCorsPolicy");
app.UseRouting();


app.UseIdentityServer();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();