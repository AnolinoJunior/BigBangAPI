using System.ComponentModel.DataAnnotations;

namespace BigBangAPI.Models
{
    public class CharacterEpisode
    {
        [Key]
        public int CharacterId { get; set; }

        public Character? Character { get; set; }

        public int EpisodeId { get; set; }

        public Episode? Episode { get; set; }
    }
}