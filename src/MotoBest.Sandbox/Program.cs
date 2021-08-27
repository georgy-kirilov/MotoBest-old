namespace MotoBest.Sandbox
{
    using System;
    using System.Text;
    using System.Threading.Tasks;

    using AngleSharp;

    using MotoBest.Data;
    using MotoBest.Services;
    using MotoBest.Seeding.Seeders;
    using MotoBest.Scraping.Scrapers;

    public class Program
    {
        public static async Task Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            var appSeeder = new ApplicationSeeder();
            await appSeeder.SeedAsync(new ApplicationDbContext());

            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);

            await SeedAdvertsAsync(new MobileBgWebScraper(context));
        }

        public static async Task SeedAdvertsAsync(BaseWebScraper scraper)
        {
            await new ApplicationDbContext().Database.EnsureCreatedAsync();

            await scraper.ScrapeAllAdvertsAsync(async model =>
            {
                using var dbContext = new ApplicationDbContext();
                var service = new AdvertsService(dbContext, new ModelFactory(dbContext), new AdvertsFormatter());
                await service.AddOrUpdateAdvertAsync(model);
            });
        }
    }
}
