using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MPD.Auth.Models.ViewModels;
using MPD.Core.Data;
using MPD.Core.Entities;
using MPD.Core.ExternalProviderServices;

namespace MPD.Auth.Controllers;

public class AuthController : Controller
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly ISpotifyProviderService _spotifyProviderService;

    public AuthController(SignInManager<User> signInManager, ISpotifyProviderService spotifyProviderService,
        UserManager<User> userManager)
    {
        _signInManager = signInManager;
        _spotifyProviderService = spotifyProviderService;
        _userManager = userManager;
    }

    public async Task<IActionResult> ExternalProvider(string provider, string returnUrl)
    {
        try
        {
            // if (!Url.IsLocalUrl(returnUrl))
            // {
            //     return View("");
            // }

            var redirect = Url.Action(nameof(ExternalLoginCallback), "Auth", new
            {
                returnUrl = returnUrl
            });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirect);

            return Challenge(properties, provider);
        }
        catch (Exception e)
        {
            throw;
        }
    }

    public async Task<IActionResult> ExternalLoginCallback(string returnUrl)
    {
        try
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction($"Login?returnUrl={returnUrl}");
            }

            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
                await _signInManager.SignInAsync(user, false);
                return Redirect(returnUrl);
            }

            await _spotifyProviderService.RegisterUser(info);
            var newUser = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
            await _signInManager.SignInAsync(newUser, false);
            return Redirect(returnUrl);
        }
        catch (Exception e)
        {
            throw;
        }
    }

    [HttpGet]
    public async Task<IActionResult> Login(string returnUrl = "/")
    {
        var externalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        return View("Login", new LoginModel()
        {
            ReturnUrl = returnUrl,
            ExternalProviders = externalLogins
        });
    }
}