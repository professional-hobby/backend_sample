namespace FlowerSpotCore.ModelsEndpoints.Sightings
{
    public class SightingOutputModel
    {
        public int SightingId { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; } = "";

        public int FlowerId { get; set; }

        public string FlowerName { get; set; } = "";

        public float Latitude { get; set; }

        public float Longitude { get; set; }

        public string QuoteOfDay { get; set; } = "";
    }
}
