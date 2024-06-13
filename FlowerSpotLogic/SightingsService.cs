using AutoMapper;
using FlowerSpotCore.InterfacesRepository;
using FlowerSpotCore.InterfacesServices;
using FlowerSpotCore.InterfacesServices.ExternalAPIs;
using FlowerSpotCore.ModelsEndpoints.Sightings;
using FlowerSpotCore.ModelsRepository;
using Microsoft.EntityFrameworkCore;

namespace FlowerSpotLogic
{
    public class SightingsService : ISightingsService
    {
        private IAllRepositories _allRepositories;
        private IQuoteOfTheDayService _quoteOfTheDayService;

        private IMapper _mapperSightingSightingOutput;

        public SightingsService(IAllRepositories AllRepositories, IQuoteOfTheDayService QuoteOfTheDayService)
        {
            _allRepositories = AllRepositories;
            _quoteOfTheDayService = QuoteOfTheDayService;


            _mapperSightingSightingOutput = new MapperConfiguration(cfg => cfg.CreateMap<SightingModel, SightingOutputModel>()
                .ForMember(dest => dest.UserName, o => o.MapFrom(s => (s.User ?? new UserModel("", "", "", "")).UserName ?? ""))
                .ForMember(dest => dest.FlowerName, o => o.MapFrom(s => (s.Flower ?? new FlowerModel("", "", "")).Name ?? ""))
                ).CreateMapper();
        }

        public async Task<bool> AddSightingAsync(int UserID, AddSightingModel AddSightingModel)
        {
            string quoteOfDay = await _quoteOfTheDayService.GetQuoteOfTheDayAsync();

            _allRepositories.Sightings.Create(new SightingModel(UserID, AddSightingModel.FlowerId, AddSightingModel.Latitude, AddSightingModel.Longitude, quoteOfDay));

            int result = _allRepositories.Save();

            return result == 1;
        }

        public bool DeleteSighting(int UserID, DeleteSightingModel DeleteSightingModel)
        {
            SightingModel? deletingSighting = findSighting(DeleteSightingModel.SightingId);

            if (deletingSighting != null && deletingSighting.UserId.Equals(UserID))
            {
                _allRepositories.Sightings.Delete(deletingSighting);
                int result = _allRepositories.Save();

                if (result == 1) //if sighting is deleted, delete also all likes of this Sighting
                {
                    List<LikeModel> likes = _allRepositories.Likes.FindByCondition(x => x.SightingId.Equals(deletingSighting.SightingId)).ToList();

                    foreach (LikeModel like in likes)
                    {
                        _allRepositories.Likes.Delete(like);
                    }

                    _allRepositories.Save();
                }

                return result == 1;
            }
            else
            {
                return false;
            }
        }

        public SightingOutputModel? GetSighting(GetSightingModel GetSightingModel)
        {
            SightingModel? sighting = findSighting(GetSightingModel.SightingId);

            return _mapperSightingSightingOutput.Map<SightingOutputModel>(sighting);
        }

        public List<SightingOutputModel> GetSightingsForFlowerId(GetSightingsForFlowerIdModel Flower)
        {
            List<SightingModel> sightings = _allRepositories.Sightings.FindByCondition(x => x.FlowerId.Equals(Flower.FlowerId)).Include(f => f.Flower).Include(u => u.User).ToList();

            return _mapperSightingSightingOutput.Map<List<SightingOutputModel>>(sightings);
        }

        public List<SightingOutputModel> GetSightingsForFlowerName(GetSightingsForFlowerNameModel Flower)
        {
            List<SightingModel> sightings = _allRepositories.Sightings.FindByCondition(x => x.Flower != null && x.Flower.Name.Equals(Flower.FlowerName)).Include(f => f.Flower).Include(u => u.User).ToList();

            return _mapperSightingSightingOutput.Map<List<SightingOutputModel>>(sightings);
        }

        private SightingModel? findSighting(int SightingId)
        {
            List<SightingModel> selectedSightings = _allRepositories.Sightings.FindByCondition(x => x.SightingId.Equals(SightingId)).Include(f => f.Flower).Include(u => u.User).ToList();

            if (selectedSightings.Count == 1)
            {
                return selectedSightings[0];
            }
            else
            {
                return null;
            }
        }
    }
}
