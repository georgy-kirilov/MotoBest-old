namespace MotoBest.Data.Scraping.Scrapers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using AngleSharp;
    using AngleSharp.Dom;

    using MotoBest.Data.Scraping.Common;

    using static MotoBest.Data.Scraping.Common.Utilities;
    using static MotoBest.Data.Scraping.Common.Utilities.Date;
    using static MotoBest.Data.Scraping.Common.Utilities.Currency;
    using static MotoBest.Data.Scraping.Common.Utilities.Characters;
    using static MotoBest.Data.Scraping.Common.ScrapedDataNormalizer;

    using static MotoBest.Data.Scraping.Common.PropertyParser;

    public class CarmarketBgWebScraper : BaseWebScraper
    {
        public const string CarmarketBgAdvertProviderName = "carmarket.bg";
        public const string CarmarketBgAdvertUrlFormat = "https://www.carmarket.bg/{0}";
        public const string AdvertSearchUrlFormat = "https://www.carmarket.bg/obiavi/{0}?sort=1";

        private static readonly Dictionary<string, Action<string, AdvertScrapeModel>> TechnicalParsingTable = new()
        {
            { "врати", ParseDoorsCount },
            { "цвят", ParseColorNameAndExterior },
            { "цена", (input, model) => model.Price = ParsePrice(input) },
            { "дата на производство", (input, model) => model.ManufacturingDate = ParseManufacturingDate(input) },
            { "тип двигател", (input, model) => model.EngineType = input?.Trim() },
            { "мощност", (input, model) => model.HorsePowers = ParseHorsePowers(input) },
            { "скоростна кутия", (input, model) => model.TransmissionType = input?.Trim() },
            { "категория", (input, model) => model.BodyStyleName = input?.Trim() },
            { "пробег", (input, model) => model.Kilometrage = ParseKilometrage(input) },
            { "тип", (input, model) => model.Condition = input?.Trim() },
        };

        public CarmarketBgWebScraper(IBrowsingContext browsingContext)
            : base(browsingContext, CarmarketBgAdvertUrlFormat, CarmarketBgAdvertProviderName)
        {
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
            model.IsNewImport = ScrapeImportValue(document);
            model.Views = ScrapeViews(document);

            ScrapeBrandAndModelName(document, model);
            ScrapeTechnicalCharacteristics(document, model);

            model.EuroStandardType = EstimateEuroStandard(model);
            NormalizeScrapeModel(model);

            return model;
        }

        public override async Task ScrapeAllAdvertsAsync(Action<AdvertScrapeModel> action)
        {
            int advertsPerPage = 15;
            int advertsCount = await ScrapeAllAdvertsCountAsync();
            int pagesCount = advertsCount / advertsPerPage;

            for (int pageIndex = 1; pageIndex <= pagesCount; pageIndex++)
            {
                string url = string.Format(AdvertSearchUrlFormat, pageIndex);
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

        public static DateTime? ScrapeLastModifiedOn(IDocument document)
        {
            try
            {
                var dateTimeTag = document.QuerySelector("div.cmOfferStatus > time");
                var date = DateTime.ParseExact(dateTimeTag.GetAttribute("datetime"), "yyyy-MM-dd", BulgarianCultureInfo);

                var args = dateTimeTag.TextContent.Split(Whitespace, StringSplitOptions.RemoveEmptyEntries);
                var timeArgs = args[2].Split(Colon);

                int hour = int.Parse(timeArgs[0]);
                int minute = int.Parse(timeArgs[1]);

                return new DateTime(date.Year, date.Month, date.Day, hour, minute, 0);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static HashSet<string> ScrapeImageUrls(IDocument document)
        {
            var imageUrls = new HashSet<string>();

            string bigImageUrl = document.QuerySelector("section.cmOffer > a")?.GetAttribute("href")
                ?? document.QuerySelector("section.cmOffer > img")?.GetAttribute("src");

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
                var scriptTagVariableArgs = document.QuerySelector(query)?.TextContent.Split(NewLine)[7].Split("=")[1].Split($"{Comma}{Whitespace}");

                string brandName = SanitizeText(scriptTagVariableArgs[0], "'").Trim();
                string modelName = scriptTagVariableArgs[1].Trim();

                if (rawImageTagBrandAndModel != $"{brandName}{Whitespace}{modelName}")
                {
                    return;
                }

                imageTagArgs = new string[] { brandName, modelName };
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

            foreach (var pair in technicalPairs)
            {
                if (TechnicalParsingTable.ContainsKey(pair.Key))
                {
                    TechnicalParsingTable[pair.Key].Invoke(pair.Value, model);
                }
            }
        }

        public static void ParseDoorsCount(string input, AdvertScrapeModel model)
        {
            model.HasFourDoors = SanitizeText(input, "врати").Trim() == "4(5)";
        }

        public async Task<int> ScrapeAllAdvertsCountAsync()
        {
            string query = "span.foundOffers > strong";
            string firstPageUrl = string.Format(AdvertSearchUrlFormat, string.Empty);
            var firstPageDocument = await browsingContext.OpenAsync(firstPageUrl);
            return int.Parse(SanitizeText(firstPageDocument.QuerySelector(query)?.TextContent, Whitespace));
        }
    }
}
