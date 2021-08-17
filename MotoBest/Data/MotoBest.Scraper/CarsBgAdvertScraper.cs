namespace MotoBest.Scraper
{
    using AngleSharp;
    using AngleSharp.Dom;

    using System;
    using System.Linq;
    using System.Text;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;
    using System.Text.Json.Serialization;

    using static Utilities;
    using static ScrapedDataNormalizer;

    public class CarsBgAdvertScraper : BaseAdvertScraper
    {
        public const string CarsBgAdvertUrlFormat = "https://www.cars.bg/offer/{0}";
        public const string CarsBgAdvertProviderName = "cars.bg";

        private readonly HttpClient httpClient;

        public CarsBgAdvertScraper(IBrowsingContext browsingContext) 
            : base(browsingContext, CarsBgAdvertUrlFormat, CarsBgAdvertProviderName)
        {
            httpClient = new HttpClient();
        }

        public override async Task<AdvertScrapeModel> ScrapeAdvertAsync(string remoteId)
        {
            var document = await browsingContext.OpenAsync(GetAdvertUrl(remoteId));
            var advert = await base.ScrapeAdvertAsync(remoteId);

            advert.Title = ScrapeTitle(document);
            advert.Description = ScrapeDescription(document);
            advert.Views = await ScrapeViewsAsync(remoteId);
            advert.Price = ScrapePrice(document);

            var characteristics = ScrapeTechnicalCharacteristics(document).Select(x => x.ToLower()).ToArray();

            advert.ManufacturingDate = ParseManufacturingDate(characteristics[0]);
            advert.BodyStyleName = ParseBodyStyleName(characteristics[1]);
            advert.EngineType = ParseEngineType(characteristics[2]);
            advert.Kilometrage = ParseKilometrage(characteristics[3]);
            advert.TransmissionType = ParseTransmissionType(characteristics[4]);
            advert.HorsePowers = ParseHorsePowers(characteristics[5]);

            string euroStandard = ParseEuroStandardType(characteristics[6]);

            advert.EuroStandardType = euroStandard.StartsWith("EURO") ? euroStandard : null;
            advert.ColorName = ParseColorName(characteristics[^1]);

            if (advert.EuroStandardType == null)
            {
                advert.EuroStandardType = EstimateEuroStandard(advert);
            }

            return advert;
        }

        public override async Task ScrapeAllAdvertsAsync(Action<AdvertScrapeModel> action)
        {
            throw new NotImplementedException();
        }

        public override async Task ScrapeLatestAdvertsAsync(Action<AdvertScrapeModel> action)
        {
            throw new NotImplementedException();
        }

        public async Task<int> ScrapeViewsAsync(string remoteId)
        {
            string url = $"https://stats.cars.bg/get/?object_id={remoteId}";
            var response = await httpClient.GetAsync(url);
            string content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ViewsInfo>(content).ViewsCount;
        }

        public static string ScrapeTitle(IDocument document)
        {
            return document.QuerySelector("h2")?.TextContent.Trim();
        }

        public static string ScrapeDescription(IDocument document)
        {
            return document.QuerySelector("div.offer-notes")?.TextContent.Trim();
        }

        public static decimal? ScrapePrice(IDocument document)
        {
            string input = SanitizeText(document.QuerySelector("div.offer-price > strong")?.TextContent, "лв.").Trim();

            if (input == null)
            {
                return null;
            }

            return decimal.Parse(input);
        }

        public static string[] ScrapeTechnicalCharacteristics(IDocument document)
        {
            var args = document.QuerySelector("div.text-copy > div.text-copy")?.TextContent.Split(", ");


            var stringsToReplace = new string[]
            {
                "Употребяван автомобил,",
                "нов внос,",
                "В добро състояние,",
                "2/3 врати,",
                "4/5 врати,",
            };

            var builder = new StringBuilder();

            foreach (string stringToReplace in stringsToReplace)
            {
                builder.Replace(stringToReplace, string.Empty);
            }

            return builder.ToString().Split(",", StringSplitOptions.RemoveEmptyEntries);
        }

        public static DateTime ParseManufacturingDate(string input)
        {
            string[] args = input.Trim().Split(" ", StringSplitOptions.RemoveEmptyEntries);
            int month = DateTime.ParseExact(args[0], MonthNameDateFormat, BulgarianCultureInfo).Month;
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
