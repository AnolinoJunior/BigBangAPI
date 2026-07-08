using BigBangAPI.Context;
using BigBangAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BigBangAPI.Repositories
{
    public class EpisodeRepository : IEpisodeRepository
    {
        private readonly AppDbContext _context;

        public EpisodeRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Episode> GetEpisodes()
        {
            return _context.Episodes
                .Include(e => e.CharacterEpisodes)
                .ToList();
        }

        public Episode? GetEpisode(int id)
        {
            return _context.Episodes
                .Include(e => e.CharacterEpisodes)
                .FirstOrDefault(e => e.Id == id);
        }

        public IEnumerable<Character> GetCharactersByEpisode(int id)
        {
            return _context.CharacterEpisodes
                .Where(ce => ce.EpisodeId == id)
                .Select(ce => ce.Character!)
                .ToList();
        }

        public Episode Create(Episode episode)
        {
            if (episode == null)
                throw new ArgumentNullException(nameof(episode));

            _context.Episodes.Add(episode);
            _context.SaveChanges();

            return episode;
        }

        public Episode? Update(Episode episode)
        {
            if (episode == null)
                throw new ArgumentNullException(nameof(episode));

            _context.Entry(episode).State = EntityState.Modified;
            _context.SaveChanges();

            return episode;
        }

        public Episode? Delete(int id)
        {
            var episode = _context.Episodes.Find(id);

            if (episode == null)
                return null;

            _context.Episodes.Remove(episode);
            _context.SaveChanges();

            return episode;
        }
    }
}