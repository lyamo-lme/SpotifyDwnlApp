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

    public async Task<DataFile> DownloadAudioAsync(UrlDto dto)
    {
        var streamManifest = await _youtube.Videos.Streams.GetManifestAsync(dto.Id);

        var audioStreamInfo = streamManifest
            .GetAudioStreams()
            .Where(s => s.Container == Container.WebM)
            .GetWithHighestBitrate();

        var audio = await _youtube.Videos.Streams.GetAsync(audioStreamInfo);
        using MemoryStream ms = new MemoryStream();
        await audio.CopyToAsync(ms);
        return new DataFile()
        {
            Name = dto.Name,
            dataBytes = ms.ToArray(),
            TypeFile = Container.WebM.ToString()
        };
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
                dataBytes = ms.ToArray(),
                Name = videoId.Name,
                TypeFile = Container.WebM.ToString()
            });
            await ms.DisposeAsync();
        }

        return file;
    }

    public async Task<byte[]> ToZipAsync(List<DataFile> data)
    {
        using (var ms = new MemoryStream())
        {
            using (var zipArchive = new ZipArchive(ms, ZipArchiveMode.Create, true))
            {
                foreach (var attachment in data)
                {
                    var entry = zipArchive.CreateEntry($"{attachment.Name}.{attachment.TypeFile}",
                        CompressionLevel.Fastest);
                    await using var entryStream = entry.Open();
                    await using Stream stream = new MemoryStream(attachment.dataBytes);
                    await stream.CopyToAsync(entryStream);
                }
            }

            return ms.ToArray();
        }
    }

    public async ValueTask<List<DataFile>> Convert(List<DataFile> data, string outputFormat)
    {
        var output = new List<DataFile>(data.Count());
        /*need to write to collection and dispose not auto*/
        foreach (var webmBytes in data)
        {
            using (var webmStream = new MemoryStream(webmBytes.dataBytes))
            {
                using (var mp3Stream = new MemoryStream())
                {
                    var ffMpeg = new FFMpegConverter();
                    var convertTask = ffMpeg.ConvertLiveMedia(webmStream, webmBytes.TypeFile, mp3Stream, outputFormat,
                        new ConvertSettings());
                    convertTask.Start();
                    convertTask.Wait();
                    output.Add(new DataFile()
                    {
                        dataBytes = mp3Stream.ToArray(),
                        Name = webmBytes.Name
                    });
                }
            }
        }

        return output;
    }
}