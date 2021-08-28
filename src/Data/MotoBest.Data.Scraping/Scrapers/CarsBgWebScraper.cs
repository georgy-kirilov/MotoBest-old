namespace MotoBest.Data.Scraping.Scrapers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using AngleSharp;
    using AngleSharp.Dom;

    using MotoBest.Data.Scraping.Common;
    using MotoBest.Data.Seeding.Entities;

    using static MotoBest.Data.Scraping.Common.Utilities;
    using static MotoBest.Data.Scraping.Common.Utilities.Date;
    using static MotoBest.Data.Scraping.Common.Utilities.Characters;

    public class CarsBgWebScraper : BaseWebScraper
    {
        private const string Description = "description";
        private const string NewImportKeyWord = "нов внос";

        public const string CarsBgAdvertProviderName = "cars.bg";
        public const string CarsBgAdvertUrlFormat = "https://www.cars.bg/offer/{0}";

        private static readonly Dictionary<string, Action<string, AdvertScrapeModel>> KeyValuePairsParsingTable = new()
        {
            { "година", ParseManufacturingDate },
            { "пробег", ParseKilometrage },
            { "гориво", (input, model) => model.EngineType = input },
            { "скорости", (input, model) => model.TransmissionType = input },
            { "мощност", ParseHorsePowers },
            { "брой врати", ParseDoorsCount },
            { "цвят", ParseColor },
            { Description, (input, model) => model.Description = input },
        };

        public CarsBgWebScraper(IBrowsingContext browsingContext)
            : base(browsingContext, CarsBgAdvertUrlFormat, CarsBgAdvertProviderName)
        {
        }

        public override async Task<AdvertScrapeModel> ScrapeAdvertAsync(string remoteId)
        {
            string url = GetAdvertUrl(remoteId);
            string html = await GetHtmlAsync(url, UserAgentHeader);

            await File.WriteAllTextAsync("C:/Users/georg/OneDrive/Desktop/cars-bg-advert.html", html);

            var document = await browsingContext.OpenAsync(res => res.Content(html));
            AdvertScrapeModel model = await base.ScrapeAdvertAsync(remoteId);

            ScrapeKeyValuePairsInfo(document, model);

            var properties = ScrapeTopLineProperties(document);

            model.BodyStyleName = properties[1];
            model.TownName = properties[^1];
            model.IsNewImport = properties.Contains(NewImportKeyWord);

            properties.Remove(NewImportKeyWord);
            properties.RemoveAt(properties.Count - 1);

            model.Condition = properties[^1];
            ScrapedDataNormalizer.NormalizeCondition(properties[^2]);

            NormalizeScrapeModel(model);

            return model;
        }

        public override async Task ScrapeAllAdvertsAsync(Action<AdvertScrapeModel> action)
        {
            throw new NotImplementedException();
        }

        public override async Task ScrapeLatestAdvertsAsync(Action<AdvertScrapeModel> action)
        {
            throw new NotImplementedException();
        }

        public static List<string> ScrapeTopLineProperties(IDocument document)
        {
            return document.QuerySelector("span.blur-text")?.TextContent
                           .Split(Comma, StringSplitOptions.RemoveEmptyEntries)
                           .Select(x => x.Trim())
                           .Where(x => x.Length > 0)
                           .ToList();
        }

        public static void ScrapeBrandAndModelName(IDocument document, AdvertScrapeModel model)
        {
            string input = document.QuerySelector("div.text-copy > div[style='float-left']")?.TextContent;
            var args = input.Split(Whitespace, StringSplitOptions.RemoveEmptyEntries);

            if (args.Length < 2)
            {

            }

            model.BrandName = args[0];
            model.ModelName = args[1];
        }

        public static void ScrapeKeyValuePairsInfo(IDocument document, AdvertScrapeModel model)
        {
            string query = "div.view-section.section-boredered > div.text-copy > div.small-text";

            var technicalPairs = document.QuerySelectorAll(query)
                                         .Select(ParseAsKeyValuePair)
                                         .ToDictionary(pair => pair.Key, pair => pair.Value);

            foreach (var pair in technicalPairs)
            {
                if (KeyValuePairsParsingTable.ContainsKey(pair.Key))
                {
                    KeyValuePairsParsingTable[pair.Key].Invoke(pair.Value, model);
                }
            }
        }

        private static void ParseManufacturingDate(string input, AdvertScrapeModel model)
        {
            var dateArgs = input.Split(Whitespace);

            int month = DateTime.ParseExact(dateArgs[0], MonthNameDateFormat, BulgarianCultureInfo).Month;
            int year = int.Parse(dateArgs[1]);

            model.ManufacturingDate = new DateTime(year, month, 1);
        }

        private static void ParseKilometrage(string input, AdvertScrapeModel model)
        {
            input = SanitizeText(input, Whitespace, Comma, "km");
            model.Kilometrage = long.Parse(input);
        }

        private static void ParseHorsePowers(string input, AdvertScrapeModel model)
        {
            input = SanitizeText(input, Whitespace, "к.с.");
            model.HorsePowers = int.Parse(input);
        }

        public static void ParseDoorsCount(string input, AdvertScrapeModel model)
        {
            model.HasFourDoors = input == "4/5";
        }

        public static void ParseColor(string input, AdvertScrapeModel model)
        {
            string metallic = Colors.Metallic.ToLower();

            model.IsExteriorMetallic = input.Contains(metallic);
            model.ColorName = SanitizeText(input, metallic).Trim();
        }

        private static KeyValuePair<string, string> ParseAsKeyValuePair(IElement divElement)
        {
            string key = Description, value = divElement.TextContent.Trim();

            if (value == string.Empty)
            {
                value = null;
            }

            if (!divElement.ClassList.Contains("offer-notes"))
            {
                string input = divElement.TextContent.Trim().ToLower();
                input = SanitizeText(input, NewLine.ToString());
                var args = input.Split($"{Colon}{Whitespace}");

                key = args[0].Trim();
                value = args[1].Trim();
            }

            return new KeyValuePair<string, string>(key, value);
        }
    }
}
