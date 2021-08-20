namespace MotoBest.Scraping.Scrapers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using AngleSharp;
    using AngleSharp.Dom;

    using MotoBest.Scraping.Common;

    using static MotoBest.Scraping.Common.Utilities;
    using static MotoBest.Scraping.Common.Utilities.Date;
    using static MotoBest.Scraping.Common.Utilities.Characters;
    using static MotoBest.Scraping.Common.ScrapedDataNormalizer;

    using static MotoBest.Scraping.Common.PropertyParser;

    public class MobileBgWebScraper : BaseWebScraper
    {
        public const string MobileBgAdvertProviderName = "mobile.bg";
        public const string MobileBgAdvertUrlFormat = "https://www.mobile.bg/pcgi/mobile.cgi?act=4&adv={0}";
        public const string MobileBgSearchUrlFormat = "https://www.mobile.bg/pcgi/mobile.cgi?act=3&slink=l40dr7&f1={0}";

        private static readonly Dictionary<string, Action<string, AdvertScrapeModel>> KeyValuePairsParsingTable = new()
        {
            { "цвят", ParseColorNameAndExterior },
            { "тип двигател", (input, model) => model.EngineType = input?.Trim() },
            { "скоростна кутия", (input, model) => model.TransmissionType = input?.Trim() },
            { "категория", (input, model) => model.BodyStyleName = input?.Trim() },
            { "евростандарт", (input, model) => model.EuroStandardType = input?.Trim().ToLower() },
            { "състояние", (input, model) => model.Condition = input?.Trim() },
            { "дата на производство", (input, model) => model.ManufacturingDate = ParseManufacturingDate(input) },
            { "мощност", (input, model) => model.HorsePowers = ParseHorsePowers(input) },
            { "пробег", (input, model) => model.Kilometrage = ParseKilometrage(input) },
        };

        private HashSet<string> features = new();

        public MobileBgWebScraper(IBrowsingContext browsingContext) 
            : base(browsingContext, MobileBgAdvertUrlFormat, MobileBgAdvertProviderName)
        {
        }

        public override async Task<AdvertScrapeModel> ScrapeAdvertAsync(string remoteId)
        {
            var document = await browsingContext.OpenAsync(GetAdvertUrl(remoteId));
            var model = await base.ScrapeAdvertAsync(remoteId);

            features = ScrapeFeatures(document);

            model.LastModifiedOn = ScrapeLastModifiedOn(document);

            model.Title = ScrapeTitle(document);
            model.Price = ScrapePrice(document);

            model.Views = ScrapeViews(document);
            model.Description = ScrapeDescription(document);

            model.IsNewImport = features.Contains("нов внос");
            model.HasFourDoors = features.Contains("4(5) врати");

            ScrapeBrandAndModelName(document, model);
            ScrapeTechnicalCharacteristics(document, model);

            ScrapeImageUrls(document, model);
            ScrapeRegionAndTownName(document, model);

            if (model.EuroStandardType == null)
            {
                model.EuroStandardType = EstimateEuroStandard(model);
            }

            NormalizeScrapeModel(model);

            return model;
        }

        public override async Task ScrapeAllAdvertsAsync(Action<AdvertScrapeModel> action)
        {
            int pagesCount = 150;

            for (int pageIndex = 1; pageIndex <= pagesCount; pageIndex++)
            {
                var document = await browsingContext.OpenAsync(string.Format(MobileBgSearchUrlFormat, pageIndex));
                var urls = document.QuerySelectorAll("a.mmm").Select(a => a.GetAttribute("href"));

                foreach (string url in urls)
                {
                    string remoteId = url.Split("?")[1].Split("&")[1].Split("=")[1];
                    AdvertScrapeModel model = await ScrapeAdvertAsync(remoteId);
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
            return document.QuerySelector("h1")?.TextContent.Trim();
        }

        public static string ScrapeDescription(IDocument document)
        {
            string description = document.QuerySelectorAll("form[name='search'] > table")?[2]
                                         .QuerySelector("tbody > tr > td")?
                                         .TextContent
                                         .Trim();

            return description != string.Empty ? description : null;
        }

        public static decimal? ScrapePrice(IDocument document)
        {
            return ParsePrice(document.QuerySelector("#details_price")?.TextContent);
        }

        public static void ScrapeTechnicalCharacteristics(IDocument document, AdvertScrapeModel model)
        {
            var characteristicsList = document.QuerySelectorAll("ul.dilarData > li");

            for (int i = 0; i < characteristicsList.Length; i += 2)
            {
                string propertyKey = characteristicsList[i]?.TextContent.ToLower();
                string propertyValue = characteristicsList[i + 1]?.TextContent.ToLower();

                KeyValuePairsParsingTable[propertyKey].Invoke(propertyValue, model);
            }
        }

        public static HashSet<string> ScrapeFeatures(IDocument document)
        {
            char bullet = (char) 0x2022;
            int featuresTableIndex = 2;
            var tableElements = document.QuerySelectorAll("table[width=660][cellspacing=0]");

            var allAdvertFeatures = new HashSet<string>();

            if (tableElements.Length > featuresTableIndex)
            {
                allAdvertFeatures = tableElements[featuresTableIndex]
                                        .QuerySelector("tr")?
                                        .TextContent
                                        .Split(new char[] { bullet, NewLine }, StringSplitOptions.RemoveEmptyEntries)
                                        .Select(x => x.Trim().ToLower())
                                        .ToHashSet();
            }

            return allAdvertFeatures;
        }

        public static int? ScrapeViews(IDocument document)
        {
            try
            {
                return int.Parse(SanitizeText(document.QuerySelector("span.advact")?.TextContent, Whitespace));
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static void ScrapeImageUrls(IDocument document, AdvertScrapeModel model)
        {
            var imageUrls = document.QuerySelectorAll("div#pictures_moving > a")
                                    .Select(a => a.GetAttribute("data-link"));

            foreach (string imageUrl in imageUrls)
            {
                model.ImageUrls.Add(imageUrl);
            }
        }

        public static void ScrapeBrandAndModelName(IDocument document, AdvertScrapeModel model)
        {
            var args = document
                        .QuerySelector("a.fastLinks")?
                        .GetAttribute("href")?
                        .Trim()
                        .Split("?")[1]
                        .Split("&");

            model.BrandName = args[1].Split("=")[1];
            model.ModelName = args[2].Split("=")[1];
        }

        public static void ScrapeRegionAndTownName(IDocument document, AdvertScrapeModel model)
        {
            var addressBlocks = document.QuerySelectorAll("div.adress");
            int addressBlockIndex = addressBlocks.Length > 1 ? 1 : 0;
            string fullAddress = addressBlocks[addressBlockIndex].TextContent.Trim();

            var fullAddressArgs = fullAddress.Split($"{Comma}{Whitespace}");
            model.RegionName = fullAddressArgs[0]?.Trim();
            model.TownName = SanitizeText(fullAddressArgs[1], "гр.", "с.", "к.к.").Trim();
        }

        public static DateTime? ScrapeLastModifiedOn(IDocument document)
        {
            try
            {
                string query = "div[style='float:left; margin-top:10px;'] > span";
                var args = document.QuerySelector(query)?.TextContent.Split(Whitespace);
                var timeArgs = args[2].Split(Colon);

                int hour = int.Parse(timeArgs[0]);
                int minute = int.Parse(timeArgs[1]);
                int day = int.Parse(args[5]);
                int month = DateTime.ParseExact(args[6], MonthNameDateFormat, BulgarianCultureInfo).Month;
                int year = int.Parse(args[7]);

                return new DateTime(year, month, day, hour, minute, 0);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
