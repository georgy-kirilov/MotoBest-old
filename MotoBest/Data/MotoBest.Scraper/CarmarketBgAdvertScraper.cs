namespace MotoBest.Scraper
{
    using AngleSharp;
    using AngleSharp.Dom;

    using System;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using static Utilities;

    public class CarmarketBgAdvertScraper : AdvertScraper
    {
        public const string CarmarketBgAdvertUrlFormat = "https://www.carmarket.bg/{0}";
        public const string CarmarketBgAdvertProviderName = "carmarket.bg";

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

        public CarmarketBgAdvertScraper(IBrowsingContext browsingContext) : base(CarmarketBgAdvertUrlFormat, CarmarketBgAdvertProviderName)
        {
            this.browsingContext = browsingContext;
        }

        public override async Task<AdvertScrapeModel> ScrapeAdvertAsync(string remoteId)
        {
            var document = await browsingContext.OpenAsync(GetAdvertUrl(remoteId));
            var model = await base.ScrapeAdvertAsync(remoteId);

            model.Title = ScrapeTitle(document);
            model.Description = ScrapeDescription(document);
            model.LastModifiedOn = ScrapeLastModifiedOn(document);
            model.RegionName = ScrapeRegionName(document);
            model.ImageUrls = ScrapeImageUrls(document);
            model.IsNewImport = ScrapeIsNewImportValue(document);
            model.Views = ScrapeViews(document);

            ScrapeBrandAndModelName(document, model);
            ScrapeTechnicalCharacteristics(document, model);

            return model;
        }

        public override Task ScrapeAllAdvertsAsync(string brandName, string modelName, Action<AdvertScrapeModel> action)
        {
            throw new NotImplementedException();
        }

        public override Task ScrapeLatestAdvertsAsync(Action<AdvertScrapeModel> action)
        {
            throw new NotImplementedException();
        }

        public static string ScrapeTitle(IDocument document)
        {
            return document.QuerySelector("h1[itemprop='name']")?.TextContent.Trim();
        }

        public static string ScrapeDescription(IDocument document)
        {
            return document.QuerySelector("section.cmOfferAddInfo > p")?.TextContent.Trim();
        }

        public static int ScrapeViews(IDocument document)
        {
            string viewsAsText = SanitizeInput(document.QuerySelector("p.cmOfferSeen > span")?.TextContent, Whitespace);
            return int.Parse(viewsAsText);
        }

        public static string ScrapeRegionName(IDocument document)
        {
            return SanitizeInput(document.QuerySelector("span.cmOfferRegion")?.TextContent, "Регион:").Trim();
        }

        public static DateTime ScrapeLastModifiedOn(IDocument document)
        {
            var dateTimeTag = document.QuerySelector("div.cmOfferStatus > time");
            var date = DateTime.ParseExact(dateTimeTag.GetAttribute("datetime"), "yyyy-MM-dd", BulgarianCultureInfo);
            var args = dateTimeTag.TextContent.Split(Whitespace, StringSplitOptions.RemoveEmptyEntries);
            var timeArgs = args[2].Split(":");
            int hour = int.Parse(timeArgs[0]);
            int minute = int.Parse(timeArgs[1]);
            return new DateTime(date.Year, date.Month, date.Day, hour, minute, 0);
        }

        public static HashSet<string> ScrapeImageUrls(IDocument document)
        {
            string bigImageUrl = document.QuerySelector("section.cmOffer > a").GetAttribute("href");

            var imageUrls = new HashSet<string>() { bigImageUrl };

            foreach (var item in document.QuerySelectorAll("ul.cmOfferSmallImages > li"))
            {
                string url = item.QuerySelector("a")?.GetAttribute("href");

                if (url != null)
                {
                    imageUrls.Add(url);
                }
            }

            return imageUrls;
        }

        public static bool ScrapeIsNewImportValue(IDocument document)
        {
            return document.QuerySelectorAll("section.cmOfferFeatures > ul > li").Any(x => x.TextContent.Trim() == "Нов внос");
        }

        public static void ScrapeBrandAndModelName(IDocument document, AdvertScrapeModel model)
        {
            var args = document.QuerySelector("img").GetAttribute("alt").Split(" ", StringSplitOptions.RemoveEmptyEntries);
            string modelName = args[^1], brandName = args[^2];
            model.BrandName = brandName;
            model.ModelName = modelName;
        }

        public static void ScrapeTechnicalCharacteristics(IDocument document, AdvertScrapeModel model)
        {
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

                ParsingTable[pair.Key].Invoke(pair.Value, model);
            }
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

        private static string SanitizeInput(string input, params string[] stringsToSanitize)
        {
            var builder = new StringBuilder(input);

            foreach (string stringToSanitize in stringsToSanitize)
            {
                builder.Replace(stringToSanitize, string.Empty);
            }

            return builder.ToString();
        }
    }
}
