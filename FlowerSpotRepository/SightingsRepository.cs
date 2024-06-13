using FlowerSpotCore.InterfacesRepository;
using FlowerSpotCore.ModelsRepository;
using FlowerSpotData.PostgreSQL;

namespace FlowerSpotRepository
{
    public class SightingsRepository : RepositoryBase<SightingModel>, ISightingsRepository
    {
        public SightingsRepository(AppDbContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}
