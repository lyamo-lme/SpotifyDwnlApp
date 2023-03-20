using Microsoft.AspNetCore.Identity;

namespace MPD.Core.Entities;

public class User:IdentityUser<int>
{
    public SpotifyProfile? SpotifyProfile { get; set; }
}