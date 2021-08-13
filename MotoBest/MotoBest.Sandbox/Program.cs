namespace MotoBest.Sandbox
{
    using Scraper;
    using AngleSharp;

    using MotoBest.Data;
    using MotoBest.Services;

    using System.Threading.Tasks;
    using System.Linq;
    using System;

    public class Program
    {
        public static async Task Main()
        {
            //Queries();
            //await new ApplicationDbContext().Database.EnsureDeletedAsync();
            //await new ApplicationDbContext().Database.EnsureCreatedAsync();

            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);

            AdvertScraper scraper = new CarsBgAdvertScraper(context);
            var advert = await scraper.ScrapeAdvertAsync("611637121d432f4cbf5b5ff2");
            
            //await scraper.ScrapeAllAdvertsAsync(async model =>
            //{
            //    using var dbContext = new ApplicationDbContext();
            //    var service = new AdvertsService(dbContext);
            //    await service.AddAdvertAsync(model);
            //});
        }

        public static void Queries()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            var db = new ApplicationDbContext();
            int count = db.Adverts.Count();
            var bmws = db.Adverts.Where(a => a.Brand.Name.ToLower() == "bmw").OrderByDescending(a => a.Price).ToList();

            foreach (var bmw in bmws)
            {
                Console.WriteLine(bmw.Price + " - " + bmw.Title);
            }

            Console.WriteLine(count);
        }
    }
}
