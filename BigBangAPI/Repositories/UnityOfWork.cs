using BigBangAPI.Context;

namespace BigBangAPI.Repositories;

public class UnityOfWork : IUnityOfWork
{
    private  ICharacterRepository? _characterRepository;
    private ILocationRepository? _locationRepository;
    private  IEpisodeRepository? _episodeRepository;
    public AppDbContext _context;

    public UnityOfWork(AppDbContext context)
    {
        _context = context;
    }

    public ICharacterRepository CharacterRepository
    {
        get
        {
            return _characterRepository ?? new CharacterRepository(_context);  
             //if (_characterRepository == null)
           // {
               // _characterRepository = new CharacterRepository(_context);
           // }
            //return _characterRepository;
        }
    }
    public ILocationRepository LocationRepository
    {
        get
        {
            if (_locationRepository == null)
            {
                _locationRepository = new LocationRepository(_context);
            }
            return _locationRepository;
        }
    }
    public IEpisodeRepository EpisodeRepository
    {
        get
        {
            if (_episodeRepository == null)
            {
                _episodeRepository = new EpisodeRepository(_context);
            }
            return _episodeRepository;
        }
    }

    public void Commit()
    {
        _context.SaveChanges();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}