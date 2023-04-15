using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MPD.Auth.Models.Config;
using MPD.Core.DownloadProvider;
using MPD.Services;
using SpotifyAPI.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<IMusicDownloader, YouTubeMusicProvider>();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<SpotifyClient>(x =>
{
    var clientCredentials = builder.Configuration
        .GetSection("spotify")
        .Get<ServiceSetting>();

    var config = SpotifyClientConfig
        .CreateDefault()
        .WithAuthenticator(
            new ClientCredentialsAuthenticator(
                clientCredentials.ClientId,
                clientCredentials.ClientSecret
            ));

    return new SpotifyClient(config);
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = false,
            ValidIssuer = "my-issuer",
            ValidAudience = "my-audience",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your-secret-key"))
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();