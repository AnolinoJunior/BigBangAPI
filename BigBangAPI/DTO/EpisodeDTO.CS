namespace BigBangAPI.DTO;
public class EpisodeDTO
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public int Season { get; set; }

    public int EpisodeNumber { get; set; }
}