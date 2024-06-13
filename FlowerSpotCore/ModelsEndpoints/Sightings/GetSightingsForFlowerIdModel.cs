using System.ComponentModel.DataAnnotations;

namespace FlowerSpotCore.ModelsEndpoints.Sightings
{
    public class GetSightingsForFlowerIdModel
    {
        [Required]
        [Range(0, int.MaxValue)]
        public int FlowerId { get; set; } = -1;
    }
}
