namespace MPD.Core.Entities;

public class SpotifyProfile
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; }
    public string NameIdentifier { get; set; } = "";
    public string SpotifyUrl { get; set; } = "";
    public string ProfilePictureUrl { get; set; } = "";
    public User? User { get; set; }
}