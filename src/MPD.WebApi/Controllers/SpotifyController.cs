using System.Text;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Microsoft.AspNetCore.Mvc;
using MPD.Core.DownloadProvider;
using MPD.WebApi.Models;
using SpotifyAPI.Web;
using YouTubeApiRestClient.Views;
using YoutubeExplode;
using YoutubeExplode.Common;
using YouTubeSearch;

namespace MPD.WebApi.Controllers;

[ApiController]
[Route("api/spotify")]
public class SpotifyController : Controller
{
    private readonly SpotifyClient _spotifyClient;
    private readonly IMusicDownloader _musicDownloader;
    private readonly YoutubeClient _youtubeClient;

    public SpotifyController(SpotifyClient spotifyClient, IMusicDownloader musicDownloader)
    {
        _spotifyClient = spotifyClient;
        _musicDownloader = musicDownloader;
        _youtubeClient = new YoutubeClient();
    }

    [HttpGet]
    public async Task<IActionResult> Get(string findValue)
    {
        var playlist = await _spotifyClient.Playlists.Get(findValue);
        var tracks = await _spotifyClient.PaginateAll(playlist.Tracks);
        List<UrlDto> urls = new List<UrlDto>();
        foreach (var track in tracks)
        {
            if (track.Track is FullTrack trackItem)
            {
                StringBuilder queryBuilder = new StringBuilder();
                foreach (var artist in trackItem.Artists)
                {
                    queryBuilder.Append(artist.Name);
                    queryBuilder.Append(" "); 
                }

                queryBuilder.Append(trackItem.Name);
                
                var items = new VideoSearch();
                var videoUrl = (await items.GetVideosPaged(queryBuilder.ToString(), 1))[0].getUrl();
                urls.Add(new UrlDto()
                {
                    Id = videoUrl,
                    Name = queryBuilder.ToString()
                });
            }
        }

        return Ok(urls);
    }

    public async Task<IActionResult> SearchResult(string query)
    {
        var searchResult = await _spotifyClient.Search.Item(new SearchRequest(SearchRequest.Types.All, query));
        return Ok(searchResult);
    }
}