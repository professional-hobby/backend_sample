using FlowerSpotCore.InterfacesRepository;
using FlowerSpotCore.InterfacesServices;
using FlowerSpotCore.ModelsRepository;

namespace FlowerSpotLogic
{
    public class FlowersService : IFlowersService
    {
        private IAllRepositories _allRepositories;

        public FlowersService(IAllRepositories AllRepositories)
        {
            _allRepositories = AllRepositories;
        }

        public List<FlowerModel> GetAllFlowers()
        {
            return _allRepositories.Flowers.GetAll().ToList();
        }

        public FlowerModel? GetFlower(int FlowerId)
        {
            List<FlowerModel> flower = _allRepositories.Flowers.FindByCondition(x => x.FlowerId.Equals(FlowerId)).ToList();

            if (flower.Count() > 0)
            {
                return flower[0];
            }
            else
            {
                return null;
            }
        }

        public bool AddFlower(FlowerModel Flower)
        {
            if (!_allRepositories.Flowers.FindByCondition(x => x.Name.Equals(Flower.Name)).Any())
            {
                _allRepositories.Flowers.Create(Flower);

                int result = _allRepositories.Save();

                return result == 1;
            }
            return false;
        }
    }
}
