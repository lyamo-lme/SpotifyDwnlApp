using System.Security.Claims;

namespace MPD.Services.Claims;

public static class SpotifyClaimTypes
{
        const string  ClaimType2005Namespace = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims";
        public const string ProfilePictureUrl = "urn:spotify:url";
        public const string SpotifyUrl = "urn:spotify:profilepicture";
}