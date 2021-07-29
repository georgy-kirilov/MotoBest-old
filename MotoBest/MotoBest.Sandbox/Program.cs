namespace MotoBest.Sandbox
{
    using System;
    using Scraper;
    using AngleSharp;
    using System.Text;
    using AngleSharp.Dom;
    using System.Threading.Tasks;
    using MotoBest.Data;
    using MotoBest.Services;
    using Models;
    using System.Linq;

    public class Program
    {
        public static async Task Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            DatabaseConfig.IsDatabaseLocal = false;

            var dbContext = new ApplicationDbContext();
            //await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.EnsureCreatedAsync();

            var advertsService = new AdvertsService(dbContext);

            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);
            
            var address = "https://www.mobile.bg/pcgi/mobile.cgi?act=3&slink=kvo3k4&f1=";
            var query = "a.mmm";

            var mobileBgProvider = new AdvertProvider
            {
                Name = "mobile.bg",
                AdvertUrlFormat = "https://www.mobile.bg/pcgi/mobile.cgi?act=4&adv={0}",
            };

            //await dbContext.AdvertProviders.AddAsync(mobileBgProvider);
            //await dbContext.SaveChangesAsync();

            for (int page = 6; page <= 10; page++)
            {
                var document = await context.OpenAsync($"{address}{page}");
                var anchorTags = document.QuerySelectorAll(query);

                foreach (IElement anchorTag in anchorTags)
                {
                    string url = anchorTag.GetAttribute("href").Trim();
                    var advertDocument = await context.OpenAsync($"https:{url}");
                    var scrapeModel = MobileBgAdvertScraper.Scrape(advertDocument, url);
                    scrapeModel.AdvertProviderName = mobileBgProvider.Name;
                    await advertsService.AddAdvertAsync(scrapeModel);
                    dbContext.SaveChanges();
                }
            }
        }
    }
}
