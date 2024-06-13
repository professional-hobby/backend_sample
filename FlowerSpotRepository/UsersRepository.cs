using FlowerSpotCore.InterfacesRepository;
using FlowerSpotCore.ModelsRepository;
using FlowerSpotData.PostgreSQL;

namespace FlowerSpotRepository
{
    public class UsersRepository : RepositoryBase<UserModel>, IUsersRepository
    {
        public UsersRepository(AppDbContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}
