namespace BigBangAPI.Repositories
{
    public interface IUnityOfWork
    {
        ICharacterRepository CharacterRepository { get; }
        ILocationRepository LocationRepository { get; }
        IEpisodeRepository EpisodeRepository { get; }
        void Commit();
    }
}