using FlowerSpotCore.InterfacesServices.ExternalAPIs;
using FlowerSpotQuoteOfTheDayProvider.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowerSpotQuoteOfTheDayProvider
{
    public class QuoteOfTheDayService : IQuoteOfTheDayService
    {
        private string _serverUrl;
        private string _serverApiKey;
        private DateTime _quoteOfTheDayValidDate;
        private string _quoteOfTheDayValue;

        public QuoteOfTheDayService()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json");

            var config = configuration.Build();

            _serverUrl = config.GetSection("QuoteOfTheDay:Url").Value ?? "";
            _serverApiKey = config.GetSection("QuoteOfTheDay:ApiKey").Value ?? "";

            _quoteOfTheDayValidDate = DateTime.MinValue;
            _quoteOfTheDayValue = "";
        }

        public async Task<string> GetQuoteOfTheDayAsync()
        {
            if (_quoteOfTheDayValidDate != DateTime.UtcNow.Date)
            {
                QuoteOfTheDayModel? quoteOfTheDay = await aquireQuoteOfTheDayFromServerAsync();
                if (quoteOfTheDay?.Success?.Total > 0 && quoteOfTheDay.Contents?.Quotes?.Count > 0 && quoteOfTheDay.Contents.Quotes[0].Quote != null && quoteOfTheDay.Contents.Quotes[0].Date != null)
                {
                    _quoteOfTheDayValue = quoteOfTheDay.Contents.Quotes[0].Quote??"";
                    try
                    {
                        _quoteOfTheDayValidDate = DateTime.Parse(quoteOfTheDay.Contents.Quotes[0].Date ?? "");
                    }
                    catch
                    {
                        _quoteOfTheDayValidDate = DateTime.MinValue;
                    }
                }
            }

            return _quoteOfTheDayValue;
        }

        private async Task<QuoteOfTheDayModel?> aquireQuoteOfTheDayFromServerAsync()
        {
            string content = "";
            QuoteOfTheDayModel? result;

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _serverApiKey);

                    // Make a GET request to the API
                    HttpResponseMessage response = await client.GetAsync(_serverUrl);

                    // Check if the request was successful (status code 200-299)
                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content
                        content = await response.Content.ReadAsStringAsync();
                    }
                }
                catch
                {
                }
            }

            if (content.Length > 0)
            {
                result = JsonConvert.DeserializeObject<Models.QuoteOfTheDayModel>(content);
            }
            else
            {
                result = null;
            }

            return result;
        }
    }
}
