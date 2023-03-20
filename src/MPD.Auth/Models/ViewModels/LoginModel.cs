using Microsoft.AspNetCore.Authentication;

namespace MPD.Auth.Models.ViewModels;

public class LoginModel
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string ReturnUrl { get; set; }
    public IList<AuthenticationScheme>? ExternalProviders { get; set;}
}