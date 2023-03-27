using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MPD.Core.Entities;

namespace MPD.Data.DbContext;

public class AppDbContext:IdentityDbContext<User, IdentityRole<int>, int>
{
    public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
    {
        // Database.EnsureDeleted();
        // Database.EnsureCreated();
    }
    public DbSet<User> Users { get; set; }
    public DbSet<SpotifyProfile> SpotifyProfiles { get; set; }
}