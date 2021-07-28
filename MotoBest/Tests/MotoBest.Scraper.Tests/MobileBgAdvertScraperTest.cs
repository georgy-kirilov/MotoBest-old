namespace MotoBest.Scraper.Tests
{
    using System.IO;
    using System.Linq;
    using System.Text.Json;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using NUnit.Framework;

    using AngleSharp;
    using AngleSharp.Dom;

    [TestFixture]
    public class MobileBgAdvertScraperTest
    {
        private const string BmwPath = "./Resources/MobileBg/Bmw";
        private const string RenaultPath = "./Resources/MobileBg/Renault";
        private const string HondaPath = "./Resources/MobileBg/Honda";

        private const string HondaUrl = "https://www.mobile.bg/pcgi/mobile.cgi?act=4&adv=11607256624769900";

        private readonly HashSet<string> paths = new()
        {
            BmwPath,
            RenaultPath,
            HondaPath,
        };

        private readonly Dictionary<string, IDocument> pathDocumentPairs = new();
        private readonly Dictionary<string, AdvertScrapeModel> pathScrapeModelPairs = new();
        private readonly AdvertScrapeModel originalScrapeModel = new();

        [OneTimeSetUp]
        public async Task InitializePathDocumentPairsAsync()
        {
            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);

            foreach (string path in paths)
            {
                string content = await File.ReadAllTextAsync($"{path}/RawAdvert.html");
                IDocument document = await context.OpenAsync(x => x.Content(content));
                pathDocumentPairs.Add(path, document);
            }
        }

        [OneTimeSetUp]
        public async Task InitializePathScrapeModelPairsAsync()
        {
            foreach (string path in paths)
            {
                using FileStream openStream = File.OpenRead($"{path}/ScrapedAdvert.json");
                AdvertScrapeModel scrapeModel = await JsonSerializer.DeserializeAsync<AdvertScrapeModel>(openStream);
                pathScrapeModelPairs.Add(path, scrapeModel);
            }
        }

        [TestCase(HondaPath)]
        public void ScrapeTitle_Should_Work_Correctly(string path)
        {
            MobileBgAdvertScraper.ScrapeTitle(pathDocumentPairs[path], originalScrapeModel);
            Assert.AreEqual(pathScrapeModelPairs[path].Title, originalScrapeModel.Title);
        }

        [TestCase(HondaPath)]
        public void ScrapeDescription_Should_Work_Correctly(string path)
        {
            MobileBgAdvertScraper.ScrapeDescription(pathDocumentPairs[path], originalScrapeModel);
            Assert.AreEqual(pathScrapeModelPairs[path].Description, originalScrapeModel.Description);
        }

        [TestCase(HondaPath)]
        public void ScrapePrice_Should_Work_Correctly(string path)
        {
            MobileBgAdvertScraper.ScrapePrice(pathDocumentPairs[path], originalScrapeModel);
            Assert.AreEqual(pathScrapeModelPairs[path].Price, originalScrapeModel.Price);
        }

        [TestCase(HondaPath)]
        public void ScrapeTechnicalCharacteristics_Should_WorkCorrectly(string path)
        {
            var scrapeModel = pathScrapeModelPairs[path];
            MobileBgAdvertScraper.ScrapeTechnicalCharacteristics(pathDocumentPairs[path], originalScrapeModel);

            Assert.AreEqual(scrapeModel.HorsePowers, originalScrapeModel.HorsePowers);
            Assert.AreEqual(scrapeModel.ManufacturingDate, originalScrapeModel.ManufacturingDate);
            Assert.AreEqual(scrapeModel.EngineType, originalScrapeModel.EngineType);
            Assert.AreEqual(scrapeModel.TransmissionType, originalScrapeModel.TransmissionType);
            Assert.AreEqual(scrapeModel.BodyStyleName, originalScrapeModel.BodyStyleName);
            Assert.AreEqual(scrapeModel.Kilometrage, originalScrapeModel.Kilometrage);
            Assert.AreEqual(scrapeModel.ColorName, originalScrapeModel.ColorName);
            Assert.AreEqual(scrapeModel.EuroStandardType, originalScrapeModel.EuroStandardType);
        }

        [TestCase(HondaPath)]
        public void ScrapeViews_Should_Work_Correctly(string path)
        {
            MobileBgAdvertScraper.ScrapeViews(pathDocumentPairs[path], originalScrapeModel);
            Assert.AreEqual(pathScrapeModelPairs[path].Views, originalScrapeModel.Views);
        }

        [TestCase(HondaPath)]
        public void ScrapeBrandAndModelName_Should_Work_Correctly(string path)
        {
            MobileBgAdvertScraper.ScrapeBrandAndModelName(pathDocumentPairs[path], originalScrapeModel);
            Assert.AreEqual(pathScrapeModelPairs[path].BrandName, originalScrapeModel.BrandName);
            Assert.AreEqual(pathScrapeModelPairs[path].ModelName, originalScrapeModel.ModelName);
        }

        [TestCase(HondaPath)]
        public void ScrapeLastModifiedOn_Should_Work_Correctly(string path)
        {
            MobileBgAdvertScraper.ScrapeLastModifiedOn(pathDocumentPairs[path], originalScrapeModel);
            Assert.AreEqual(pathScrapeModelPairs[path].LastModifiedOn, originalScrapeModel.LastModifiedOn);
        }

        [TestCase(HondaPath, HondaUrl)]
        public void ParseRemoteId_Should_Work_Correctly(string path, string url)
        {
            MobileBgAdvertScraper.ParseRemoteId(url, originalScrapeModel);
            Assert.AreEqual(pathScrapeModelPairs[path].RemoteId, originalScrapeModel.RemoteId);
        }

        [TestCase(HondaPath)]
        public void ScrapeRegionAndTownName_Should_Work_Correctly(string path)
        {
            MobileBgAdvertScraper.ScrapeRegionAndTownName(pathDocumentPairs[path], originalScrapeModel);
            Assert.AreEqual(pathScrapeModelPairs[path].RegionName, originalScrapeModel.RegionName);
            Assert.AreEqual(pathScrapeModelPairs[path].TownName, originalScrapeModel.TownName);
        }

        [TestCase(HondaPath)]
        public void ScrapeImageUrls_Should_Work_Correctly(string path)
        {
            MobileBgAdvertScraper.ScrapeImageUrls(pathDocumentPairs[path], originalScrapeModel);
            Assert.IsTrue(pathScrapeModelPairs[path].ImageUrls.SetEquals(originalScrapeModel.ImageUrls));
        }
    }
}
