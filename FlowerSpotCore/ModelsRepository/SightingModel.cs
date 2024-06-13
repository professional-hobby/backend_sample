using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlowerSpotCore.ModelsRepository
{
    public class SightingModel
    {
        [Key]
        public int SightingId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int FlowerId { get; set; }

        [Required]
        public float Latitude { get; set; }

        [Required]
        public float Longitude { get; set; }

        [Required]
        public string QuoteOfDay { get; set; } = "";

        [ForeignKey("UserId")]
        public UserModel? User { get; set; }

        [ForeignKey("FlowerId")]
        public FlowerModel? Flower { get; set; }


        public SightingModel(int UserId, int FlowerId, float Latitude, float Longitude, string QuoteOfDay)
        {
            this.UserId = UserId;
            this.FlowerId = FlowerId;
            this.Latitude = Latitude;
            this.Longitude = Longitude;
            this.QuoteOfDay = QuoteOfDay;
        }
    }
}
