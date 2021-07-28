namespace MotoBest.Scraper.Tests
{
    using System.IO;
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
            var document = pathDocumentPairs[path];
            var scrapeModel = pathScrapeModelPairs[path];

            MobileBgAdvertScraper.ScrapeTitle(document, originalScrapeModel);
            Assert.AreEqual(scrapeModel.Title, originalScrapeModel.Title);
        }

        [TestCase(HondaPath)]
        public void ScrapeDescription_Should_Work_Correctly(string path)
        {
            var document = pathDocumentPairs[path];
            var scrapeModel = pathScrapeModelPairs[path];

            MobileBgAdvertScraper.ScrapeDescription(document, originalScrapeModel);
            Assert.AreEqual(scrapeModel.Description, originalScrapeModel.Description);
        }
    }
}
