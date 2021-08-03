namespace MotoBest.Scraper
{
    using AngleSharp;
    using AngleSharp.Dom;

    using System;
    using System.Text.Json;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Text.Json.Serialization;

    using static Utilities;

    public class CarsBgWebScraper : WebScraper
    {
        private const string CarsBgAdvertUrlFormat = "https://www.cars.bg/offer/{0}";

        private readonly IBrowsingContext browsingContext;
        private readonly HttpClient httpClient;

        public CarsBgWebScraper(IBrowsingContext browsingContext) : base(CarsBgAdvertUrlFormat)
        {
            httpClient = new HttpClient();
            this.browsingContext = browsingContext;
        }

        public override async Task<AdvertScrapeModel> ScrapeAdvertAsync(string remoteId)
        {
            var document = await browsingContext.OpenAsync(GetAdvertUrl(remoteId));

            var advert = new AdvertScrapeModel
            {
                Title = ScrapeTitle(document),
                Description = ScrapeDescription(document),
                Views = await ScrapeViewsAsync(remoteId),
                Price = ScrapePrice(document),
            };

            var characteristics = ScrapeTechnicalCharacteristics(document);
            advert.ManufacturingDate = ParseManufacturingDate(characteristics[0]);
            advert.BodyStyleName = ParseBodyStyleName(characteristics[1]);
            advert.EngineType = ParseEngineType(characteristics[5]);
            advert.Kilometrage = ParseKilometrage(characteristics[6]);
            advert.TransmissionType = ParseTransmissionType(characteristics[7]);
            advert.HorsePowers = ParseHorsePowers(characteristics[8]);
            advert.EuroStandardType = ParseEuroStandardType(characteristics[9]);

            return advert;
        }

        public override async Task ScrapeAllAdvertsAsync(string brandName, string modelName, Action<AdvertScrapeModel> action)
        {
            throw new NotImplementedException();
        }

        public override async Task ScrapeLatestAdvertsAsync(Action<AdvertScrapeModel> action)
        {
            throw new NotImplementedException();
        }

        public string ScrapeTitle(IDocument document)
        {
            return document.QuerySelector("h2")?.TextContent.Trim();
        }

        public async Task<int> ScrapeViewsAsync(string remoteId)
        {
            string url = $"https://stats.cars.bg/get/?object_id={remoteId}";
            var response = await httpClient.GetAsync(url);
            string content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ViewsInfo>(content).ViewsCount;
        }

        public string ScrapeDescription(IDocument document)
        {
            return document.QuerySelector("div.offer-notes")?.TextContent.Trim();
        }

        public decimal? ScrapePrice(IDocument document)
        {
            string input = document.QuerySelector("div.offer-price > strong")?.TextContent.Replace("лв.", string.Empty).Trim();

            if (input == null)
            {
                return null;
            }

            return decimal.Parse(input);
        }

        public string[] ScrapeTechnicalCharacteristics(IDocument document)
        {
            return document.QuerySelector("div.text-copy > div.text-copy")?.TextContent.Split(",");
        }

        public static DateTime ParseManufacturingDate(string input)
        {
            string[] args = input.Trim().Split(" ", StringSplitOptions.RemoveEmptyEntries);
            int month = DateTime.ParseExact(args[0], FullMonthNameDateFormat, BulgarianCultureInfo).Month;
            int year = int.Parse(args[1]);
            return new DateTime(year, month, 1);
        }

        public static string ParseBodyStyleName(string input)
        {
            return input?.Trim();
        }

        public static string ParseEngineType(string input)
        {
            return input?.Trim();
        }

        public static int ParseKilometrage(string input)
        {
            return int.Parse(input?.Replace("км", string.Empty).Replace(" ", string.Empty));
        }

        public static string ParseTransmissionType(string input)
        {
            return input?.Trim();
        }

        public static int ParseHorsePowers(string input)
        {
            return int.Parse(input?.Replace("к.с.", string.Empty));
        }

        public static string ParseEuroStandardType(string input)
        {
            return input?.Trim();
        }

        public static string ParseColorName(string input)
        {
            return input?.Trim();
        }

        private class ViewsInfo
        {
            [JsonPropertyName("value_resettable")]
            public int ViewsCount { get; set; }
        }
    }
}
