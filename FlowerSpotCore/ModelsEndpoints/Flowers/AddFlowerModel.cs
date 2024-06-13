using System.ComponentModel.DataAnnotations;

namespace FlowerSpotCore.ModelsEndpoints.Flowers
{
    public class AddFlowerModel
    {
        [Required]
        public string Name { get; set; } = "";

        [Required]
        public string ImageRef { get; set; } = "";

        [Required]
        public string Description { get; set; } = "";

    }
}
