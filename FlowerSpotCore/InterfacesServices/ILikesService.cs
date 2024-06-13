using FlowerSpotCore.ModelsEndpoints.Likes;

namespace FlowerSpotCore.InterfacesServices
{
    public interface ILikesService
    {
        public int CountLikes(CountLikesModel model);
        public bool AddLike(int UserID, AddLikeModel model);
        public bool DeleteLike(int UserID, DeleteLikeModel model);

    }
}
