namespace BigBangAPI.DTO
{
    public class CharacterDTO
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Actor { get; set; } = string.Empty;

        public string Occupation { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }
    }
}