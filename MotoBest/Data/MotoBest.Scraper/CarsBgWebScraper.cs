namespace MotoBest.Scraper
{
    using AngleSharp;
    using AngleSharp.Dom;

    using System;
    using System.Text.Json;
    using System.Net.Http;
    using System.Threading.Tasks;

    using static Utilities;

    public class CarsBgWebScraper : IWebScraper
    {
        private readonly IBrowsingContext browsingContext;
        private readonly string advertSubUrl;

        public CarsBgWebScraper(IBrowsingContext browsingContext, string advertSubUrl)
        {
            this.browsingContext = browsingContext;
            this.advertSubUrl = advertSubUrl;
        }

        public async Task<AdvertScrapeModel> ScrapeAdvertAsync(string remoteId)
        {
            var document = await browsingContext.OpenAsync($"{advertSubUrl}{remoteId}");

            var advert = new AdvertScrapeModel
            {
                Title = ScrapeTitle(document),
                Description = ScrapeDescription(document),
                Views = await ScrapeViews(remoteId),
            };

            return advert;
        }

        public async Task ScrapeAllAdvertsAsync(string brandName, string modelName, Action<AdvertScrapeModel> action)
        {
            throw new NotImplementedException();
        }

        public async Task ScrapeLatestAdvertsAsync(Action<AdvertScrapeModel> action)
        {
            throw new NotImplementedException();
        }

        public static string ScrapeTitle(IDocument document)
        {
            return document.QuerySelector("h2")?.TextContent.Trim();
        }

        public static async Task<int> ScrapeViews(string remoteId)
        {
            var client = new HttpClient();
            var response = await client.GetAsync("https://stats.cars.bg/get/?object_id=" + remoteId);
            string content = await response.Content.ReadAsStringAsync();
            var json = JsonSerializer.Deserialize<Json>(content);
            return json.value_resettable;
        }

        public static string ScrapeDescription(IDocument document)
        {
            return document.QuerySelector("div.offer-notes")?.TextContent.Trim();
        }

        public static DateTime ScrapeManufacturingDate(string monthName, string yearAsText)
        {
            int month = DateTime.ParseExact(monthName.Trim(), FullMonthNameDateFormat, BulgarianCultureInfo).Month;
            int year = int.Parse(yearAsText);
            return new DateTime(year, month, 1);
        }

        private class Json
        {
            public int value_resettable { get; set; }
        }
    }
}
