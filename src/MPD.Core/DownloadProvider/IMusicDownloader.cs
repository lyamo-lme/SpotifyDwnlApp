
namespace MPD.Core.DownloadProvider;

public interface IMusicDownloader
{
    Task<byte[]> DownloadAudioAsync(string videoId);
    Task<IEnumerable<byte[]>> DownloadAudiosAsync(string[] videoIds);
    Task<byte[]> ToZipAsync(IEnumerable<byte[]> data, string typesFile);
    ValueTask<IEnumerable<byte[]>> Convert(IEnumerable<byte[]> data, string outputFormat);
}