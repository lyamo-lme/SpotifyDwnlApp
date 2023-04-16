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
    public async Task<IActionResult> DownloadAudio(string url)
    {
        byte[] mp3Bytes = await _musicDownloader.DownloadAudioAsync(url);
        return File(mp3Bytes, "audio/webm", "mm.webm");
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
        // var mp3S = await _musicDownloader.Convert(files, Format.ogg);
        var ar = await _musicDownloader.ToZipAsync(files, Format.webm);

        return File(ar, "application/zip", "zip");
    }
}