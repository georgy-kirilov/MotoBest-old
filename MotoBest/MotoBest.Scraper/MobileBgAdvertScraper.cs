namespace MotoBest.Scraper
{
    using System;
    using System.Linq;
    using System.Globalization;
    using System.Collections.Generic;

    using AngleSharp.Dom;

    public static class MobileBgAdvertScraper
    {
        private delegate void TechnicalCharacteristicsParser(string input, AdvertScrapeModel scrapeModel);

        private static readonly Dictionary<string, TechnicalCharacteristicsParser> CharacteristicsParsingTable = new()
        {
            { "дата на производство", ParseManufacturingDate },
            { "тип двигател", ParseEngineType },
            { "мощност", ParseHorsePowers },
            { "скоростна кутия", ParseTransmissionType },
            { "категория", ParseBodyStyle },
            { "пробег", ParseKilometrage },
            { "цвят", ParseColorName },
            { "евростандарт", ParseEuroStandard },
        };

        public static AdvertScrapeModel Scrape(IDocument document, string advertisementUrl)
        {
            var scrapeModel = new AdvertScrapeModel();

            ScrapeRemoteId(advertisementUrl, scrapeModel);
            ScrapeLastModifiedOn(document, scrapeModel);

            ScrapeTitle(document, scrapeModel);
            ScrapeBrandAndModelName(document, scrapeModel);

            ScrapeTechnicalCharacteristics(document, scrapeModel);
            ScrapePrice(document, scrapeModel);

            ScrapeViews(document, scrapeModel);
            ScrapeImageUrls(document, scrapeModel);

            ScrapeDescription(document, scrapeModel);
            ScrapeRegionAndTownName(document, scrapeModel);

            return scrapeModel;
        }

        private static void ScrapeRemoteId(string advertisementUrl, AdvertScrapeModel scrapeModel)
        {
            scrapeModel.RemoteId = advertisementUrl.Split("?")[1].Split("&")[1].Split("=")[1];
        }

        private static void ScrapeTitle(IDocument document, AdvertScrapeModel scrapeModel)
        {
            scrapeModel.Title = document.QuerySelector("h1")?.TextContent;
        }

        private static void ScrapeDescription(IDocument document, AdvertScrapeModel scrapeModel)
        {
            scrapeModel.Description = document
                                        .QuerySelectorAll("form[name='search'] > table")[2]
                                        .QuerySelector("tbody > tr > td")?
                                        .TextContent;
        }

        private static void ScrapePrice(IDocument document, AdvertScrapeModel scrapeModel)
        {
            try
            {
                string input = document
                                .QuerySelector("#details_price")?
                                .TextContent
                                .Replace("лв.", string.Empty)
                                .Replace(" ", string.Empty);

                scrapeModel.Price = decimal.Parse(input);
            }
            catch (Exception)
            {
                scrapeModel.Price = null;
            }
        }

        private static void ScrapeTechnicalCharacteristics(IDocument document, AdvertScrapeModel scrapeModel)
        {
            var characteristicsList = document.QuerySelectorAll("ul.dilarData > li");

            for (int i = 0; i < characteristicsList.Length; i += 2)
            {
                string currentPropertyName = characteristicsList[i].TextContent.ToLower();
                string currentPropertyValue = characteristicsList[i + 1].TextContent;

                CharacteristicsParsingTable[currentPropertyName].Invoke(currentPropertyValue, scrapeModel);
            }
        }

        private static void ScrapeViews(IDocument document, AdvertScrapeModel scrapeModel)
        {
            scrapeModel.Views = int.Parse(document.QuerySelector("span.advact")?.TextContent);
        }

        private static void ScrapeImageUrls(IDocument document, AdvertScrapeModel scrapeModel)
        {
            var imageUrls = document
                             .QuerySelectorAll("div#pictures_moving > a")
                             .Select(a => a.GetAttribute("data-link"));

            foreach (string imageUrl in imageUrls)
            {
                scrapeModel.ImageUrls.Add(imageUrl);
            }
        }

        private static void ScrapeBrandAndModelName(IDocument document, AdvertScrapeModel scrapeModel)
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

        private static void ScrapeRegionAndTownName(IDocument document, AdvertScrapeModel scrapeModel)
        {
            var addressBlocks = document.QuerySelectorAll("div.adress");
            int addressBlockIndex = addressBlocks.Length > 1 ? 1 : 0;
            string fullAddress = addressBlocks[addressBlockIndex].TextContent.Trim();

            var fullAddressArgs = fullAddress.Split(", ");
            scrapeModel.RegionName = fullAddressArgs[0];
            scrapeModel.TownName = fullAddressArgs[1];
        }

        private static void ScrapeLastModifiedOn(IDocument document, AdvertScrapeModel scrapeModel)
        {
            var args = document.QuerySelector("div[style='float:left; margin-top:10px;'] > span")?.TextContent.Split(" ");
            var timeArgs = args[2].Split(":");

            int hour = int.Parse(timeArgs[0]);
            int minute = int.Parse(timeArgs[1]);
            int day = int.Parse(args[5]);
            int month = DateTime.ParseExact(args[6], "MMMM", Utilities.BulgarianCultureInfo).Month;
            int year = int.Parse(args[7]);

            scrapeModel.LastModifiedOn = new DateTime(year, month, day, hour, minute, 0);
        }

        private static void ParseManufacturingDate(string input, AdvertScrapeModel scrapeModel)
        {
            if (input == null)
            {
                return;
            }

            string rawDateInput = input.Replace("г.", string.Empty);
            string[] rawDateInputArgs = rawDateInput.Split(" ");

            int month = DateTime.ParseExact(rawDateInputArgs[0], "MMMM", new CultureInfo("bg-BG")).Month;
            int year = int.Parse(rawDateInputArgs[1]);

            scrapeModel.ManufacturingDate = new DateTime(year, month, 1);
        }

        private static void ParseKilometrage(string input, AdvertScrapeModel scrapeModel)
        {
            if (input == null)
            {
                return;
            }

            int kilometrage = int.Parse(input
                                         .Replace(" ", string.Empty)
                                         .ToLower()
                                         .Replace("км", string.Empty));

            scrapeModel.Kilometrage = kilometrage;
        }

        private static void ParseHorsePowers(string input, AdvertScrapeModel scrapeModel)
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

        private static void ParseEngineType(string input, AdvertScrapeModel scrapeModel)
        {
            scrapeModel.EngineType = input?.Trim();
        }

        private static void ParseTransmissionType(string input, AdvertScrapeModel scrapeModel)
        {
            scrapeModel.TransmissionType = input?.Trim();
        }

        private static void ParseBodyStyle(string input, AdvertScrapeModel scrapeModel)
        {
            scrapeModel.BodyStyleName = input?.Trim();
        }

        private static void ParseColorName(string input, AdvertScrapeModel scrapeModel)
        {
            scrapeModel.ColorName = input?.Trim();
        }

        private static void ParseEuroStandard(string input, AdvertScrapeModel scrapeModel)
        {
            scrapeModel.EuroStandardType = input?.Trim();
        }
    }
}
