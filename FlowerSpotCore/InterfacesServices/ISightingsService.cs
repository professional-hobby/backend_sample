using FlowerSpotCore.ModelsEndpoints.Sightings;

namespace FlowerSpotCore.InterfacesServices
{
    public interface ISightingsService
    {
        public Task<bool> AddSightingAsync(int UserID, AddSightingModel Sighting);
        public SightingOutputModel? GetSighting(GetSightingModel Sighting);
        public List<SightingOutputModel> GetSightingsForFlowerId(GetSightingsForFlowerIdModel Flower);
        public List<SightingOutputModel> GetSightingsForFlowerName(GetSightingsForFlowerNameModel Flower);
        public bool DeleteSighting(int UserID, DeleteSightingModel Sighting);
    }
}
