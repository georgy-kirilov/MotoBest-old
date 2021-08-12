namespace MotoBest.Sandbox
{
    using Scraper;
    using AngleSharp;

    using MotoBest.Data;
    using MotoBest.Services;

    using System.Threading.Tasks;

    public class Program
    {
        public static async Task Main()
        {
            await new ApplicationDbContext().Database.EnsureCreatedAsync();

            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);

            AdvertScraper scraper = new CarsBgWebScraper(context);
            var advert = await scraper.ScrapeAdvertAsync("600305de963dfb15a5465b82");

            /*
            await scraper.ScrapeAllAdvertsAsync(async model =>
            {
                using var dbContext = new ApplicationDbContext();
                var service = new AdvertsService(dbContext);
                await service.AddAdvertAsync(model);
            });
            */
        }
    }
}
