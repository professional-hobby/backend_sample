namespace FlowerSpotCore.InterfacesRepository
{
    public interface IAllRepositories
    {
        IFlowersRepository Flowers { get; }
        ILikesRepository Likes { get; }
        ISightingsRepository Sightings { get; }
        IUsersRepository Users { get; }
        int Save();
    }
}
