using FlowerSpotCore.InterfacesRepository;
using FlowerSpotData.PostgreSQL;

namespace FlowerSpotRepository
{
    public class AllRepositories : IAllRepositories
    {
        private AppDbContext _appDbContext;
        private IFlowersRepository? _flowersRepository;
        private ILikesRepository? _likesRepository;
        private ISightingsRepository? _sightingsRepository;
        private IUsersRepository? _usersRepository;

        public AllRepositories(AppDbContext AppDbContext)
        {
            _appDbContext = AppDbContext;
        }

        public IFlowersRepository Flowers
        {
            get
            {
                _flowersRepository ??= new FlowersRepository(_appDbContext);
                return _flowersRepository;
            }
        }

        public ILikesRepository Likes
        {
            get
            {
                _likesRepository ??= new LikesRepository(_appDbContext);
                return _likesRepository;
            }
        }

        public ISightingsRepository Sightings
        {
            get
            {
                _sightingsRepository ??= new SightingsRepository(_appDbContext);
                return _sightingsRepository;
            }
        }

        public IUsersRepository Users
        {
            get
            {
                _usersRepository ??= new UsersRepository(_appDbContext);
                return _usersRepository;
            }
        }

        public int Save()
        {
            return _appDbContext.SaveChanges();
        }
    }
}
