using System.Diagnostics;
using System.IO.Compression;
using MediaToolkit;
using MediaToolkit.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;
using YoutubeExplode.Converter;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using MPD.Core.DownloadProvider;
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
    public async Task<IActionResult> DownloadAudio(string[] url)
    {
        var files = await _musicDownloader.DownloadAudiosAsync(url);
        var mp3S = await _musicDownloader.Convert(files, Format.ogg);
        var ar =  await _musicDownloader.ToZipAsync(mp3S, Format.ogg);

        return File(ar, "application/zip", "zip");
    }
}