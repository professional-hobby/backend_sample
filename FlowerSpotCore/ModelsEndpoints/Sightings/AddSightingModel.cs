using System.ComponentModel.DataAnnotations;

namespace FlowerSpotCore.ModelsEndpoints.Sightings
{
    public class AddSightingModel
    {
        [Required]
        [Range(0, int.MaxValue)]
        public int FlowerId { get; set; } = -1;

        [Required]
        [Range(-90, 90)]
        public float Latitude { get; set; } = -91;

        [Required]
        [Range(-180, 180)]
        public float Longitude { get; set; } = -181;

    }
}
