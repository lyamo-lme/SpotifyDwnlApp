using System.Collections.Specialized;
using System.Diagnostics;
using System.IO.Compression;
using System.Web;
using MediaToolkit;
using MediaToolkit.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;
using YoutubeExplode.Converter;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using MPD.Core;
using MPD.Core.DownloadProvider;
using MPD.WebApi.Models;
using NReco.VideoConverter;
using VideoLibrary;

namespace MPD.WebApi.Controllers;

[ApiController]
[Route("api/download")]
public class DownloadController : Controller
{
    private readonly IMusicDownloader _musicDownloader;

    public DownloadController(IMusicDownloader musicDownloader)
    {
        _musicDownloader = musicDownloader;
    }

    [HttpGet]
    [Route("{url}")]
    public async Task<IActionResult> DownloadAudio(UrlDto dto)
    {
        DataFile musicFile = await _musicDownloader.DownloadAudioAsync(dto);
        return File(musicFile.dataBytes, $"audio/{musicFile.TypeFile}", $"{musicFile.Name}");
    }

    [HttpPost]
    [Route("")]
    public async Task<IActionResult> DownloadAudio(UrlDto[] urls)
    {
        foreach (var url in urls)
        {
            NameValueCollection parameters = HttpUtility.ParseQueryString(new Uri(url.Id).Query);
            url.Id = parameters["v"];
        }

        var files = await _musicDownloader.DownloadAudiosAsync(urls);
        var ar = await _musicDownloader.ToZipAsync(files);

        return File(ar, "application/zip", "zip");
    }
}