using Microsoft.AspNetCore.Mvc;
using SpotifyAPI.Web;

namespace MPD.WebApi.Controllers;

[ApiController]
[Route("api/spotify")]
public class SpotifyController:Controller
{
    private readonly SpotifyClient _spotifyClient;
    // private readonly IMusicDownloader _musicDownloader;
    public SpotifyController(SpotifyClient spotifyClient)
    {
        _spotifyClient = spotifyClient;
    }

    [HttpGet] 
    public async Task<IActionResult> Get(string findValue)
    {
        var searchResult = await _spotifyClient.Search.Item(new SearchRequest(SearchRequest.Types.All, findValue));
        return Ok(searchResult);
    }
}