namespace FlowerSpotCore.InterfacesServices.ExternalAPIs
{
    public interface IQuoteOfTheDayService
    {
        public Task<string> GetQuoteOfTheDayAsync();
    }
}
