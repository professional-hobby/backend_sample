using System.ComponentModel.DataAnnotations;

namespace FlowerSpotCore.ModelsRepository
{
    public class FlowerModel
    {
        [Key]
        public int FlowerId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string ImageRef { get; set; }

        [Required]
        public string Description { get; set; }

        public FlowerModel(string Name, string ImageRef, string Description)
        {
            this.Name = Name;
            this.ImageRef = ImageRef;
            this.Description = Description;
        }
    }
}
