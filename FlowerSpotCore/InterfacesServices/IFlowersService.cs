using FlowerSpotCore.ModelsRepository;

namespace FlowerSpotCore.InterfacesServices
{
    public interface IFlowersService
    {
        public List<FlowerModel> GetAllFlowers();
        public FlowerModel? GetFlower(int FlowerId);
        public bool AddFlower(FlowerModel Flower);
    }
}
