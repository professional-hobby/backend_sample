using FlowerSpotCore.InterfacesRepository;
using FlowerSpotCore.ModelsRepository;
using FlowerSpotData.PostgreSQL;

namespace FlowerSpotRepository
{
    public class FlowersRepository : RepositoryBase<FlowerModel>, IFlowersRepository
    {
        public FlowersRepository(AppDbContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}
