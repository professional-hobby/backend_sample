namespace FlowerSpotQuoteOfTheDayProvider.Models
{
    public class QuoteModel
    {
        public string? Quote { get; set; }
        public int Length { get; set; }
        public string? Author { get; set; }
        public List<string>? Tags { get; set; }
        public string? Category { get; set; }
        public string? Date { get; set; }
        public string? Title { get; set; }
        public string? Background { get; set; }
        public string? ID { get; set; }
    }
}
