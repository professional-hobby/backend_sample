using System.ComponentModel.DataAnnotations;

namespace FlowerSpotCore.ModelsEndpoints.Sightings
{
    public class GetSightingsForFlowerNameModel
    {
        [Required]
        public string FlowerName { get; set; } = "";
    }
}
