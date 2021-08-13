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
            //await new ApplicationDbContext().Database.EnsureDeletedAsync();
            await new ApplicationDbContext().Database.EnsureCreatedAsync();

            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);

            AdvertScraper scraper = new MobileBgAdvertScraper(context);
            var model = await scraper.ScrapeAdvertAsync("11622188754581401");
            await new AdvertsService(new ApplicationDbContext()).AddAdvertAsync(model);
            
            /*await scraper.ScrapeAllAdvertsAsync(async model =>
            {
                using var dbContext = new ApplicationDbContext();
                var service = new AdvertsService(dbContext);
                await service.AddAdvertAsync(model);
            });*/
        }
    }
}
