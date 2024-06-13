using System.ComponentModel.DataAnnotations;

namespace FlowerSpotCore.ModelsEndpoints.Sightings
{
    public class DeleteSightingModel
    {
        [Required]
        [Range(0, int.MaxValue)]
        public int SightingId { get; set; } = -1;

    }
}
