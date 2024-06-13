using FlowerSpotCore.InterfacesRepository;
using FlowerSpotCore.ModelsRepository;
using FlowerSpotData.PostgreSQL;

namespace FlowerSpotRepository
{
    public class LikesRepository : RepositoryBase<LikeModel>, ILikesRepository
    {
        public LikesRepository(AppDbContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}
