using BigBangAPI.Models;

namespace BigBangAPI.Repositories
{
    public interface IEpisodeRepository
    {
        IEnumerable<Episode> GetEpisodes();

        Episode? GetEpisode(int id);

        IEnumerable<Character> GetCharactersByEpisode(int id);

        Episode Create(Episode episode);

        Episode? Update(Episode episode);

        Episode? Delete(int id);
    }
}