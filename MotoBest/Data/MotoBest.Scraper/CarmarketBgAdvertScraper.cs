namespace MotoBest.Scraper
{
    using AngleSharp;
    using AngleSharp.Dom;

    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using static Utilities;

    public class CarmarketBgAdvertScraper : AdvertScraper
    {
        public const string CarmarketBgAdvertUrlFormat = "https://www.carmarket.bg/{0}";
        public const string CarmarketBgAdvertProviderName = "carmarket.bg";
        public const string AdvertSearchUrlFormat = "https://www.carmarket.bg/obiavi/{0}?sort=1";

        private readonly IBrowsingContext browsingContext;

        private static readonly Dictionary<string, Action<string, AdvertScrapeModel>> TechnicalParsingTable = new()
        {
            { "цена", ParsePrice },
            { "дата на производство", ParseManufacturingDate },
            { "тип двигател", ParseEngineType },
            { "мощност", ParseHorsePowers },
            { "скоростна кутия", ParseTransmissionType },
            { "категория", ParseBodyStyleName },
            { "пробег", ParseKilometrage },
            { "тип", ParseCondition },
            { "врати", ParseDoors },
            { "цвят", ParseColorName },
        };

        private static readonly SortedDictionary<DateTime, string> EuroStandardsByDateTable = new()
        {
            { new DateTime(1992, 12, 31), "Евро 1" },
            { new DateTime(1997, 1, 1), "Евро 2" },
            { new DateTime(2001, 1, 1), "Евро 3" },
            { new DateTime(2006, 1, 1), "Евро 4" },
            { new DateTime(2011, 1, 1), "Евро 5" },
            { new DateTime(2015, 9, 1), "Евро 6" },
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
            model.LastModifiedOn = ScrapeLastModifiedOn(document, remoteId);
            model.RegionName = ScrapeRegionName(document);
            model.ImageUrls = ScrapeImageUrls(document);
            model.IsNewImport = ScrapeImportValue(document);
            model.Views = ScrapeViews(document);

            ScrapeBrandAndModelName(document, model);
            ScrapeTechnicalCharacteristics(document, model);
            ParseEuroStandard(model);

            return model;
        }

        public override async Task ScrapeAllAdvertsAsync(Action<AdvertScrapeModel> action)
        {
            string firstPageUrl = string.Format(AdvertSearchUrlFormat, string.Empty);
            var firstPageDocument = await browsingContext.OpenAsync(firstPageUrl);

            int advertsCount = int.Parse(
                                    firstPageDocument.QuerySelector("span.foundOffers > strong")?
                                            .TextContent
                                            .Replace(Whitespace, string.Empty));

            for (int advertIndex = 200; advertIndex < advertsCount / 15; advertIndex++)
            {
                string url = string.Format(AdvertSearchUrlFormat, advertIndex);
                var document = await browsingContext.OpenAsync(url);

                var ids = document.QuerySelectorAll("div.cmOffersList > div.cmOffersListItem")
                                  .Select(x => x.QuerySelector("a")
                                                .GetAttribute("href")
                                                .Split("?")[0]
                                                .Split("/")[^1]);
                foreach (string id in ids)
                {
                    AdvertScrapeModel model = await ScrapeAdvertAsync(id);
                    action.Invoke(model);
                }
            }
        }

        public override Task ScrapeLatestAdvertsAsync(Action<AdvertScrapeModel> action)
        {
            //TODO: Implement ScrapeLatestAdvertsAsync
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
            string viewsAsText = SanitizeText(document.QuerySelector("p.cmOfferSeen > span")?.TextContent, Whitespace);
            return int.Parse(viewsAsText);
        }

        public static string ScrapeRegionName(IDocument document)
        {
            return SanitizeText(document.QuerySelector("span.cmOfferRegion")?.TextContent, "Регион:").Trim();
        }

        public static DateTime ScrapeLastModifiedOn(IDocument document, string remoteId)
        {
            try
            {
                var dateTimeTag = document.QuerySelector("div.cmOfferStatus > time");
                var date = DateTime.ParseExact(dateTimeTag.GetAttribute("datetime"), "yyyy-MM-dd", BulgarianCultureInfo);
                var args = dateTimeTag.TextContent.Split(Whitespace, StringSplitOptions.RemoveEmptyEntries);
                var timeArgs = args[2].Split(":");
                int hour = int.Parse(timeArgs[0]);
                int minute = int.Parse(timeArgs[1]);
                return new DateTime(date.Year, date.Month, date.Day, hour, minute, 0);
            }
            catch (Exception e)
            {
                Console.WriteLine(remoteId);
                Console.WriteLine(e.Message);
                return DateTime.Now;
            }
        }

        public static HashSet<string> ScrapeImageUrls(IDocument document)
        {
            var bigImageTag = document.QuerySelector("section.cmOffer > a") ?? document.QuerySelector("section.cmOffer > img");
            string bigImageUrl = bigImageTag.HasAttribute("href") ? bigImageTag.GetAttribute("href") : bigImageTag.GetAttribute("src");

            var imageUrls = new HashSet<string>();

            if (bigImageUrl != null)
            {
                imageUrls.Add(bigImageUrl);
            }

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

        public static bool ScrapeImportValue(IDocument document)
        {
            return document.QuerySelectorAll("section.cmOfferFeatures > ul > li").Any(x => x.TextContent.Trim() == "Нов внос");
        }

        public static void ScrapeBrandAndModelName(IDocument document, AdvertScrapeModel model)
        {
            string stringToReplace = "Вижте всички снимки за";

            string rawImageTagBrandAndModel = SanitizeText(
                                                document.QuerySelector("img").GetAttribute("alt"), stringToReplace)?.Trim();

            var imageTagArgs = rawImageTagBrandAndModel.Split(Whitespace, StringSplitOptions.RemoveEmptyEntries);

            if (imageTagArgs.Length != 2)
            {
                string query = "div[style='margin-top: 10px;'] > script";
                var scriptTagVariableArgs = document.QuerySelector(query)?.TextContent.Split(NewLine)[7].Split("=")[1].Split(", ");

                string brandName = SanitizeText(scriptTagVariableArgs[0], "'").Trim();
                string modelName = scriptTagVariableArgs[1].Trim();

                if (rawImageTagBrandAndModel == $"{brandName}{Whitespace}{modelName}")
                {
                    imageTagArgs = new string[] { brandName, modelName };
                }
                else
                {
                    return;
                }
            }

            model.BrandName = imageTagArgs[0];
            model.ModelName = imageTagArgs[1];
        }

        public static void ScrapeTechnicalCharacteristics(IDocument document, AdvertScrapeModel model)
        {
            var technicalPairs = document
                                    .QuerySelectorAll("div.cmOfferMoreInfo > div.cmOfferMoreInfoRow")
                                    .ToDictionary(x => x.QuerySelector("span")?.TextContent.Trim().ToLower(),
                                                  x => x.QuerySelector("strong")?.TextContent.Trim().ToLower());

            foreach (var technicalPair in technicalPairs)
            {
                if (!TechnicalParsingTable.ContainsKey(technicalPair.Key))
                {
                    continue;
                }

                TechnicalParsingTable[technicalPair.Key].Invoke(technicalPair.Value, model);
            }
        }

        public static void ParseEuroStandard(AdvertScrapeModel model)
        {
            string currentEuroStandardType = null;

            foreach (var euroStandardDatePair in EuroStandardsByDateTable)
            {
                if (euroStandardDatePair.Key.CompareTo(model.ManufacturingDate) > 0)
                {
                    break;
                }

                currentEuroStandardType = euroStandardDatePair.Value;
            }

            model.EuroStandardType = currentEuroStandardType;
            model.IsEuroStandardExact = model.EuroStandardType == null;
        }

        public static void ParsePrice(string input, AdvertScrapeModel model)
        {
            if (input == null)
            {
                model.Price = null;
            }
            else
            {
                decimal currencyExchangeRate = input.Contains("eur") ? EuroToBgnExchangeRate : 1;
                input = SanitizeText(input, "лв.", "bgn", "eur", Whitespace);
                model.Price = decimal.Parse(input) * currencyExchangeRate;
            }
        }

        public static void ParseManufacturingDate(string input, AdvertScrapeModel model)
        {
            input = SanitizeText(input, "г.")?.Trim();
            var rawDateArgs = input.Split(Whitespace, StringSplitOptions.RemoveEmptyEntries);
            int month = DateTime.ParseExact(rawDateArgs[0], MonthNameDateFormat, BulgarianCultureInfo).Month;
            int year = int.Parse(rawDateArgs[1]);
            model.ManufacturingDate = new DateTime(year, month, 1);
        }

        public static void ParseEngineType(string input, AdvertScrapeModel model)
        {
            model.EngineType = input;
        }

        public static void ParseHorsePowers(string input, AdvertScrapeModel model)
        {
            input = SanitizeText(input, "к.с.")?.Trim();
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
            input = SanitizeText(input, "км", Whitespace);
            model.Kilometrage = int.Parse(input);
        }

        public static void ParseCondition(string input, AdvertScrapeModel model)
        {
            model.Condition = input;
        }

        public static void ParseDoors(string input, AdvertScrapeModel model)
        {
            model.HasFourDoors = SanitizeText(input, "врати").Trim() == "4(5)";
        }

        public static void ParseColorName(string input, AdvertScrapeModel model)
        {
            model.ColorName = input;
        }
    }
}
