using MPD.WebApi.Models;

namespace MPD.Core.DownloadProvider;

public interface IMusicDownloader
{
    Task<DataFile> DownloadAudioAsync(UrlDto dto);
    Task<List<DataFile>> DownloadAudiosAsync(UrlDto[] dtos);
    Task<byte[]> ToZipAsync(List<DataFile> data);
    ValueTask<List<DataFile>> Convert(List<DataFile> data, string outputFormat);
}