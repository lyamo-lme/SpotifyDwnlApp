using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MPD.Core.Data;
using MPD.Core.Entities;
using MPD.Core.ExternalProviderServices;
using MPD.Services.Claims;

namespace MPD.Services.ExternalProviders;

public class SpotifyProviderService : ISpotifyProviderService
{
    private readonly ILogger<SpotifyProviderService> _logger;
    private readonly UserManager<User> _userManager;
    private readonly IUnitOfWorkRepository _unitOfWorkRepository;

    public SpotifyProviderService(UserManager<User> userManager, IUnitOfWorkRepository unitOfWorkRepository,
        ILogger<SpotifyProviderService> logger)
    {
        _userManager = userManager;
        _unitOfWorkRepository = unitOfWorkRepository;
        _logger = logger;
    }

    public async Task<User?> RegisterUser(ExternalLoginInfo info)
    {
        try
        {
            var spotifyProfile = await GetUserByClaims(info.Principal);
            var user = new User()
            {
                UserName = spotifyProfile.Name
            };

            var resultRegister = await _userManager.CreateAsync(user);

            if (!resultRegister.Succeeded)
            {
                return null;
            }

            var loginResult = await _userManager.AddLoginAsync(user, info);

            if (!loginResult.Succeeded)
            {
                return null;
            }

            spotifyProfile.UserId = user.Id;

            var newProfile = await _unitOfWorkRepository
                .GenericRepository<SpotifyProfile>()
                .CreateAsync(spotifyProfile);

            await _unitOfWorkRepository.SaveAsync();
            user.SpotifyProfile = newProfile;

            return user;
        }
        catch (Exception e)
        {
            _logger.Log(LogLevel.Error, e, e.Message);
            throw;
        }
    }

    public async Task<SpotifyProfile> GetUserByClaims(ClaimsPrincipal claimsPrincipal)
    {
        try
        {
            var profile =
                new SpotifyProfile()
                {
                    Name = claimsPrincipal.FindFirst(ClaimTypes.Name).Value,
                    NameIdentifier = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier).Value,
                    ProfilePictureUrl = claimsPrincipal.FindFirst(SpotifyClaimTypes.ProfilePictureUrl).Value,
                    SpotifyUrl = claimsPrincipal.FindFirst(SpotifyClaimTypes.SpotifyUrl).Value
                };
            
            return profile;
        }
        catch (Exception e)
        {
            throw;
        }
    }
}