
using MPD.WebApi.Models;

namespace MPD.Core.DownloadProvider;

public interface IMusicDownloader
{
    Task<byte[]> DownloadAudioAsync(string videoId);
    Task<List<DataFile>> DownloadAudiosAsync(UrlDto[] dtos);
    Task<byte[]> ToZipAsync(List<DataFile> data, string typesFile);
    ValueTask<List<DataFile>> Convert(List<DataFile> data, string outputFormat);
}