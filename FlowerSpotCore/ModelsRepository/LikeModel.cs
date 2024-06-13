using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlowerSpotCore.ModelsRepository
{
    public class LikeModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int SightingId { get; set; }

        [ForeignKey("UserId")]
        public UserModel? User { get; set; }

        [ForeignKey("SightingId")]
        public SightingModel? Sighting { get; set; }

        public LikeModel(int UserId, int SightingId)
        {
            this.UserId = UserId;
            this.SightingId = SightingId;
        }
    }
}
