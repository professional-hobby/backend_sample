using System.ComponentModel.DataAnnotations;

namespace FlowerSpotCore.ModelsEndpoints.Likes
{
    public class AddLikeModel
    {
        [Required]
        [Range(0, int.MaxValue)]
        public int SightingId { get; set; } = -1;
    }
}
