namespace MotoBest.Scraper
{
    using AngleSharp;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using static Utilities;

    public class CarmarketBgAdvertScraper : AdvertScraper
    {


        public const string CarmarketBgAdvertUrlFormat = "https://www.carmarket.bg/{0}";
        private readonly IBrowsingContext browsingContext;

        private static readonly Dictionary<string, Action<string, AdvertScrapeModel>> ParsingTable = new()
        {
            { "цена", ParsePrice },
            { "дата на производство", ParseManufacturingDate },
            { "тип двигател", ParseEngineType },
            { "мощност", ParseHorsePowers },
            { "скоростна кутия", ParseTransmissionType },
            { "категория", ParseBodyStyleName },
            { "пробег", ParseKilometrage },
            { "тип", ParseCondition },
            { "врати", ParseDoorsAsText },
            { "цвят", ParseColorName },
        };

        

        private static string SanitizeInput(string input, params string[] stringsToSanitize)
        {
            var builder = new StringBuilder(input);

            foreach (string stringToSanitize in stringsToSanitize)
            {
                builder.Replace(stringToSanitize, string.Empty);
            }

            return builder.ToString();
        }

        public CarmarketBgAdvertScraper(IBrowsingContext browsingContext) : base(CarmarketBgAdvertUrlFormat)
        {
            this.browsingContext = browsingContext;
        }

        public override async Task<AdvertScrapeModel> ScrapeAdvertAsync(string remoteId)
        {
            var advert = new AdvertScrapeModel
            {
                RemoteId = remoteId,
            };

            var document = await browsingContext.OpenAsync(GetAdvertUrl(remoteId));

            var pairs = document
                .QuerySelectorAll("div.cmOfferMoreInfo > div.cmOfferMoreInfoRow")
                .ToDictionary(x => x.QuerySelector("span")?.TextContent.Trim().ToLower(), 
                              x => x.QuerySelector("strong")?.TextContent.Trim().ToLower());

            foreach (var pair in pairs)
            {
                if (!ParsingTable.ContainsKey(pair.Key))
                {
                    continue;   
                }

                ParsingTable[pair.Key].Invoke(pair.Value, advert);
            }

            return advert;
        }

        public override Task ScrapeAllAdvertsAsync(string brandName, string modelName, Action<AdvertScrapeModel> action)
        {
            throw new NotImplementedException();
        }

        public override Task ScrapeLatestAdvertsAsync(Action<AdvertScrapeModel> action)
        {
            throw new NotImplementedException();
        }

        public static void ParsePrice(string input, AdvertScrapeModel model)
        {
            input = SanitizeInput(input, "лв.", "bgn", Whitespace);
            model.Price = decimal.Parse(input);
        }

        public static void ParseManufacturingDate(string input, AdvertScrapeModel model)
        {
            input = SanitizeInput(input, "г.")?.Trim();
            var rawDateArgs = input.Split(Whitespace, StringSplitOptions.RemoveEmptyEntries);
            int month = DateTime.ParseExact(rawDateArgs[0], FullMonthNameDateFormat, BulgarianCultureInfo).Month;
            int year = int.Parse(rawDateArgs[1]);
            model.ManufacturingDate = new DateTime(year, month, 1);
        }

        public static void ParseEngineType(string input, AdvertScrapeModel model)
        {
            model.EngineType = input;
        }

        public static void ParseHorsePowers(string input, AdvertScrapeModel model)
        {
            input = SanitizeInput(input, "к.с.")?.Trim();
            model.HorsePowers = int.Parse(input);
        }

        public static void ParseTransmissionType(string input, AdvertScrapeModel model)
        {
            model.TransmissionType = input;
        }

        public static void ParseBodyStyleName(string input, AdvertScrapeModel model)
        {
            model.BodyStyleName = input;
        }

        public static void ParseKilometrage(string input, AdvertScrapeModel model)
        {
            input = SanitizeInput(input, "км", Whitespace);
            model.Kilometrage = int.Parse(input);
        }

        public static void ParseCondition(string input, AdvertScrapeModel model)
        {
            model.Condition = input;
        }

        public static void ParseDoorsAsText(string input, AdvertScrapeModel model)
        {
            model.DoorsAsText = SanitizeInput(input, "врати").Trim();
        }

        public static void ParseColorName(string input, AdvertScrapeModel model)
        {
            model.ColorName = input;
        }
    }
}
