namespace MotoBest.Sandbox
{
    using System.Threading.Tasks;

    using AngleSharp;

    using MotoBest.Scraping.Scrapers;

    public class Program
    {
        public static async Task Main()
        {
            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);

            BaseAdvertScraper scraper = new MobileBgAdvertScraper(context);
        }
    }
}
