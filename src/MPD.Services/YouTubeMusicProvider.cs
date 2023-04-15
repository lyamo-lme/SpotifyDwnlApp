using System.IO.Compression;
using MPD.Core.DownloadProvider;
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

    public async Task<IEnumerable<byte[]>> DownloadAudiosAsync(string[] videoIds)
    {
        var file = new List<byte[]>(videoIds.Length);
        foreach (var videoId in videoIds)
        {
            var streamManifest = await _youtube.Videos.Streams.GetManifestAsync(videoId);
            var audioStreamInfo = streamManifest
                .GetAudioStreams()
                .Where(s => s.Container == Container.WebM)
                .GetWithHighestBitrate();

            var audio = await _youtube.Videos.Streams.GetAsync(audioStreamInfo);
            MemoryStream ms = new MemoryStream();
            await audio.CopyToAsync(ms);
            file.Add(ms.ToArray());
            await ms.DisposeAsync();
        }

        return file;
    }

    public async Task<byte[]> ToZipAsync(IEnumerable<byte[]> data, string typesFile)
    {
        using (var ms = new MemoryStream())
        {
            using (var zipArchive = new ZipArchive(ms, ZipArchiveMode.Create, true))
            {
                foreach (var attachment in data)
                {
                    var entry = zipArchive.CreateEntry($"some.{typesFile}", CompressionLevel.Fastest);
                    await using var entryStream = entry.Open();
                    await using Stream stream = new MemoryStream(attachment);
                    await stream.CopyToAsync(entryStream);
                }
            }

            return ms.ToArray();
        }
    }

    public async ValueTask<IEnumerable<byte[]>> Convert(IEnumerable<byte[]> data, string outputFormat)
    {
        var mp3s = new List<byte[]>(data.Count());
        foreach (var webmBytes in data)
        {
            using (var webmStream = new MemoryStream(webmBytes))
            {
                using (var mp3Stream = new MemoryStream())
                {
                    var ffMpeg = new FFMpegConverter();
                    var convertTask = ffMpeg.ConvertLiveMedia(webmStream, Format.webm, mp3Stream, outputFormat,
                        new ConvertSettings());
                    convertTask.Start();
                    convertTask.Wait();
                    mp3s.Add(mp3Stream.ToArray());
                }
            }
        }

        return mp3s;
    }
}