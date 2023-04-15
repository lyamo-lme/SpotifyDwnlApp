namespace MPD.Core.Entities;

public class SpotifyProfile
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; } = String.Empty;
    public string NameIdentifier { get; set; } = String.Empty;
    public string SpotifyUrl { get; set; }  = String.Empty;
    public string ProfilePictureUrl { get; set; } = "";
    public User? User { get; set; }
}