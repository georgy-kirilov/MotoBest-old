namespace MotoBest.Sandbox
{
    using System;
    using Scraper;
    using AngleSharp;
    using System.Text;
    using AngleSharp.Dom;
    using System.Threading.Tasks;

    public class Program
    {
        public static async Task Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);
            var address = "https://www.mobile.bg/pcgi/mobile.cgi?act=3&slink=kvo3k4&f1=";
            var query = "a.mmm";

            for (int page = 1; page <= 10; page++)
            {
                var document = await context.OpenAsync($"{address}{page}");
                var anchorTags = document.QuerySelectorAll(query);

                foreach (IElement anchorTag in anchorTags)
                {
                    string url = anchorTag.GetAttribute("href").Trim();
                    var advertisementDocument = await context.OpenAsync($"https:{url}");
                    var inputModel = MobileBgAdvertScraper.Scrape(advertisementDocument, url);
                }
            }
        }
    }
}
