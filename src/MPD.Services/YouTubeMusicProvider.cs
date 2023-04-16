using System.IO.Compression;
using MPD.Core;
using MPD.Core.DownloadProvider;
using MPD.WebApi.Models;
using NAudio.Wave;
using NReco.VideoConverter;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace MPD.Services;

public class YouTubeMusicProvider : IMusicDownloader
{
    private readonly YoutubeClient _youtube;

    public YouTubeMusicProvider()
    {
        _youtube = new YoutubeClient();
    }

    public async Task<byte[]> DownloadAudioAsync(string videoId)
    {
        var streamManifest = await _youtube.Videos.Streams.GetManifestAsync(videoId);

        var audioStreamInfo = streamManifest
            .GetAudioStreams()
            .Where(s => s.Container == Container.WebM)
            .GetWithHighestBitrate();

        var audio = await _youtube.Videos.Streams.GetAsync(audioStreamInfo);
        using MemoryStream ms = new MemoryStream();
        await audio.CopyToAsync(ms);
        return ms.ToArray();
    }

    public async Task<List<DataFile>> DownloadAudiosAsync(UrlDto[] videoIds)
    {
        var file = new List<DataFile>(videoIds.Length);
        foreach (var videoId in videoIds)
        {
            var streamManifest = await _youtube.Videos.Streams.GetManifestAsync(videoId.Id);
            var audioStreamInfo = streamManifest
                .GetAudioStreams()
                .Where(s => s.Container == Container.WebM)
                .GetWithHighestBitrate();

            var audio = await _youtube.Videos.Streams.GetAsync(audioStreamInfo);
            MemoryStream ms = new MemoryStream();
            await audio.CopyToAsync(ms);
            file.Add(new DataFile
            {
                data = ms.ToArray(),
                Name = videoId.Name
            });
            await ms.DisposeAsync();
        }

        return file;
    }

    public async Task<byte[]> ToZipAsync(List<DataFile> data, string typesFile)
    {
        using (var ms = new MemoryStream())
        {
            using (var zipArchive = new ZipArchive(ms, ZipArchiveMode.Create, true))
            {
                foreach (var attachment in data)
                {
                    var entry = zipArchive.CreateEntry($"{attachment.Name}.{typesFile}", CompressionLevel.Fastest);
                    await using var entryStream = entry.Open();
                    await using Stream stream = new MemoryStream(attachment.data);
                    await stream.CopyToAsync(entryStream);
                }
            }

            return ms.ToArray();
        }
    }

    public async ValueTask<List<DataFile>> Convert(List<DataFile> data, string outputFormat)
    {
        var output = new List<DataFile>(data.Count());
        foreach (var webmBytes in data)
        {
            using (var webmStream = new MemoryStream(webmBytes.data))
            {
                using (var mp3Stream = new MemoryStream())
                {
                    var ffMpeg = new FFMpegConverter();
                    var convertTask = ffMpeg.ConvertLiveMedia(webmStream, Format.webm, mp3Stream, outputFormat,
                        new ConvertSettings());
                    convertTask.Start();
                    convertTask.Wait();
                    output.Add(new DataFile(){
                        data = mp3Stream.ToArray(),
                        Name = webmBytes.Name
                    });
                }
            }
        }
        return output;
    }
}