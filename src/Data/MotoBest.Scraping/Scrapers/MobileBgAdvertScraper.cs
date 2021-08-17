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
    using static MotoBest.Scraping.Common.ScrapedDataNormalizer;

    public class MobileBgAdvertScraper : BaseAdvertScraper
    {
        public const string MobileBgAdvertProviderName = "mobile.bg";
        public const string MobileBgAdvertUrlFormat = "https://www.mobile.bg/pcgi/mobile.cgi?act=4&adv={0}";
        public const string MobileBgSearchUrlFormat = "https://www.mobile.bg/pcgi/mobile.cgi?act=3&slink=l40dr7&f1={0}";

        private static readonly Dictionary<string, Action<string, AdvertScrapeModel>> CharacteristicsParsingTable = new()
        {
            { "дата на производство", ParseManufacturingDate },
            { "тип двигател", ParseEngineType },
            { "мощност", ParseHorsePowers },
            { "скоростна кутия", ParseTransmissionType },
            { "категория", ParseBodyStyle },
            { "пробег", ParseKilometrage },
            { "цвят", ParseColorName },
            { "евростандарт", ParseEuroStandard },
            { "състояние", ParseCondition },
        };

        private HashSet<string> features = new();

        public MobileBgAdvertScraper(IBrowsingContext browsingContext) 
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

            model.IsNewImport = ParseImportValue();
            model.HasFourDoors = ParseDoors();

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
                    AdvertScrapeModel advert = await ScrapeAdvertAsync(remoteId);
                    action.Invoke(advert);
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
            return document.QuerySelectorAll("form[name='search'] > table")?[2]
                           .QuerySelector("tbody > tr > td")?
                           .TextContent
                           .Trim();
        }

        public static decimal? ScrapePrice(IDocument document)
        {
            try
            {
                string input = SanitizeText(document.QuerySelector("#details_price")?.TextContent, "лв.", Whitespace);
                return decimal.Parse(input);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static void ScrapeTechnicalCharacteristics(IDocument document, AdvertScrapeModel scrapeModel)
        {
            var characteristicsList = document.QuerySelectorAll("ul.dilarData > li");

            for (int i = 0; i < characteristicsList.Length; i += 2)
            {
                string currentPropertyName = characteristicsList[i].TextContent.ToLower();
                string currentPropertyValue = characteristicsList[i + 1].TextContent.ToLower();

                CharacteristicsParsingTable[currentPropertyName].Invoke(currentPropertyValue, scrapeModel);
            }
        }

        public bool ParseImportValue()
        {
            return features.Contains("нов внос");
        }

        public bool ParseDoors()
        {
            return features.Contains("4(5) врати");
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

        public static int ScrapeViews(IDocument document)
        {
            return int.Parse(document.QuerySelector("span.advact")?.TextContent);
        }

        public static void ScrapeImageUrls(IDocument document, AdvertScrapeModel scrapeModel)
        {
            var imageUrls = document
                             .QuerySelectorAll("div#pictures_moving > a")
                             .Select(a => a.GetAttribute("data-link"));

            foreach (string imageUrl in imageUrls)
            {
                scrapeModel.ImageUrls.Add(imageUrl);
            }
        }

        public static void ScrapeBrandAndModelName(IDocument document, AdvertScrapeModel scrapeModel)
        {
            var args = document
                        .QuerySelector("a.fastLinks")
                        .GetAttribute("href")
                        .Trim()
                        .Split("?")[1]
                        .Split("&");

            scrapeModel.BrandName = args[1].Split("=")[1];
            scrapeModel.ModelName = args[2].Split("=")[1];
        }

        public static void ScrapeRegionAndTownName(IDocument document, AdvertScrapeModel scrapeModel)
        {
            var addressBlocks = document.QuerySelectorAll("div.adress");
            int addressBlockIndex = addressBlocks.Length > 1 ? 1 : 0;
            string fullAddress = addressBlocks[addressBlockIndex].TextContent.Trim();

            var fullAddressArgs = fullAddress.Split(", ");
            scrapeModel.RegionName = fullAddressArgs[0];
            scrapeModel.TownName = fullAddressArgs[1];
        }

        public static DateTime ScrapeLastModifiedOn(IDocument document)
        {
            var args = document.QuerySelector("div[style='float:left; margin-top:10px;'] > span")?.TextContent.Split(" ");
            var timeArgs = args[2].Split(":");

            int hour = int.Parse(timeArgs[0]);
            int minute = int.Parse(timeArgs[1]);
            int day = int.Parse(args[5]);
            int month = DateTime.ParseExact(args[6], MonthNameDateFormat, BulgarianCultureInfo).Month;
            int year = int.Parse(args[7]);

            return new DateTime(year, month, day, hour, minute, 0);
        }

        public static void ParseManufacturingDate(string input, AdvertScrapeModel scrapeModel)
        {
            if (input == null)
            {
                return;
            }

            string rawDateInput = input.Replace("г.", string.Empty);
            string[] rawDateInputArgs = rawDateInput.Split(" ");

            int month = DateTime.ParseExact(rawDateInputArgs[0], MonthNameDateFormat, BulgarianCultureInfo).Month;
            int year = int.Parse(rawDateInputArgs[1]);

            scrapeModel.ManufacturingDate = new DateTime(year, month, 1);
        }

        public static void ParseKilometrage(string input, AdvertScrapeModel scrapeModel)
        {
            if (input == null)
            {
                return;
            }

            scrapeModel.Kilometrage = int.Parse(input
                                                 .Replace(" ", string.Empty)
                                                 .ToLower()
                                                 .Replace("км", string.Empty));
        }

        public static void ParseHorsePowers(string input, AdvertScrapeModel scrapeModel)
        {
            if (input == null)
            {
                return;
            }

            scrapeModel.HorsePowers = int.Parse(
                                            input.Replace(" ", string.Empty)
                                                 .ToLower()
                                                 .Replace("к.с.", string.Empty));
        }

        public static void ParseEngineType(string input, AdvertScrapeModel scrapeModel)
        {
            scrapeModel.EngineType = input?.Trim();
        }

        public static void ParseTransmissionType(string input, AdvertScrapeModel scrapeModel)
        {
            scrapeModel.TransmissionType = input?.Trim();
        }

        public static void ParseBodyStyle(string input, AdvertScrapeModel scrapeModel)
        {
            scrapeModel.BodyStyleName = input?.Trim();
        }

        public static void ParseColorName(string input, AdvertScrapeModel scrapeModel)
        {
            scrapeModel.ColorName = input?.Trim();
        }

        public static void ParseEuroStandard(string input, AdvertScrapeModel scrapeModel)
        {
            scrapeModel.EuroStandardType = input?.Trim().ToLower();
        }

        public static void ParseCondition(string input, AdvertScrapeModel scrapeModel)
        {
            scrapeModel.Condition = input?.Trim();
        }
    }
}
