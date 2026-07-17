namespace BigBangAPI.DTO;

public class CharacterDetailsDTO
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Actor { get; set; } = string.Empty;

    public string Occupation { get; set; } = string.Empty;

    public string University { get; set; } = string.Empty;

    public int IQ { get; set; }

    public int Age { get; set; }

    public string? LocationName { get; set; }

    public string? ImageUrl { get; set; }

    public List<string> Episodes { get; set; } = [];
}