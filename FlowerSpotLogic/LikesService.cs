using FlowerSpotCore.InterfacesRepository;
using FlowerSpotCore.InterfacesServices;
using FlowerSpotCore.ModelsEndpoints.Likes;
using FlowerSpotCore.ModelsRepository;

namespace FlowerSpotLogic
{
    public class LikesService : ILikesService
    {
        private IAllRepositories _allRepositories;
        private IUsersService _usersService;

        public LikesService(IAllRepositories AllRepositories, IUsersService UsersService)
        {
            _allRepositories = AllRepositories;
            _usersService = UsersService;
        }

        public int CountLikes(CountLikesModel LikeModel)
        {
            return _allRepositories.Likes.FindByCondition(x => x.SightingId == LikeModel.SightingId).Count();
        }

        public bool AddLike(int UserID, AddLikeModel LikeModel)
        {
            if (findLike(UserID, LikeModel.SightingId) == null)//check if Sighting already exists
            {
                _allRepositories.Likes.Create(new LikeModel(UserID, LikeModel.SightingId));

                int result = _allRepositories.Save();
                return result == 1;
            }
            else
            {
                return false;
            }
        }

        public bool DeleteLike(int UserID, DeleteLikeModel LikeModel)
        {
            LikeModel? deletingLike = findLike(UserID, LikeModel.SightingId);
            if (deletingLike != null)//check if Sighting exists
            {
                _allRepositories.Likes.Delete(deletingLike);

                int result = _allRepositories.Save();
                return result == 1;
            }
            else
            {
                return false;
            }
        }

        private LikeModel? findLike(int UserId, int SightingId)
        {
            List<LikeModel> selected = _allRepositories.Likes.FindByCondition(x => x.UserId == UserId && x.SightingId == SightingId).ToList();
            if (selected.Count > 0)
            {
                return selected[0];
            }
            else
            {
                return null;
            }
        }
    }
}
