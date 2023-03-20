using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using MPD.Core.Entities;

namespace MPD.Core.ExternalProviderServices;

public interface ISpotifyProviderService
{
    public Task<User> RegisterUser(ExternalLoginInfo model);
    public Task<SpotifyProfile> GetUserByClaims(ClaimsPrincipal claimsPrincipal);
}