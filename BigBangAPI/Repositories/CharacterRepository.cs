using BigBangAPI.Context;
using BigBangAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BigBangAPI.Repositories
{    public class CharacterRepository : ICharacterRepository
    {
        
         private readonly AppDbContext _context;

        public CharacterRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Character> GetCharacters()
        {
           return _context.Characters.Include(c => c.Location).ToList();
        }

        public Character? GetCharacter(int id)
        {
            return _context.Characters.FirstOrDefault(c => c.Id == id);
        }

        public Character Create(Character character)
        {
           if (character == null)
            {
                throw new ArgumentNullException(nameof(character));
            }

            _context.Characters.Add(character);
            _context.SaveChanges();
            return character;
        }

        public Character? Update(Character character)
        {
            if (character == null)
            
                throw new ArgumentNullException(nameof(character));

                _context.Entry(character).State = EntityState.Modified;
                _context.SaveChanges();

                return character;
            }
        
        public Character? Delete(int id)
        {
           var character = _context.Characters.Find(id);
            if (character is null)
                throw new ArgumentNullException(nameof(character));
                _context.Characters.Remove(character);
                _context.SaveChanges();
                return character;
        }
    public IEnumerable<Episode> GetEpisodesByCharacter(int id)
{
    return _context.CharacterEpisodes
        .Where(ce => ce.CharacterId == id)
        .Select(ce => ce.Episode)
        .Where(e => e != null)
        .Select(e => e!)
        .ToList();
}
    }
}
       