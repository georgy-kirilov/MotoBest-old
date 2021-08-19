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
    using System.Net.Http;
    using MotoBest.Scraping.Common;
    using AngleSharp.Io;
    using AngleSharp.Dom;
    using System.Collections.Generic;
    using System.IO;
    using System.Diagnostics;

    public class Program
    {
        private const string SamsungGalaxyS9UserAgentValue = "Mozilla/5.0 (Linux; Android 8.0.0; SM-G960F Build/R16NW) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/62.0.3202.84 Mobile Safari/537.36";
        private static readonly KeyValuePair<string, string> UserAgentHeader = new("user-agent", SamsungGalaxyS9UserAgentValue);

        public static async Task Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            //await new ApplicationDbContext().Database.EnsureDeletedAsync();
            //Console.WriteLine("Database deleted");
/*
            await new ApplicationDbContext().Database.EnsureCreatedAsync();
            Console.WriteLine("Database created");

            var appSeeder = new ApplicationSeeder();
            await appSeeder.SeedAsync(new ApplicationDbContext());

            Console.WriteLine("Seeding complete. Press any key to continue...");
            Console.ReadKey();
            Console.WriteLine("Scraping...");
*/
            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);

            string url = "https://www.cars.bg/offer/60c88084e50d624e8b1afd02";
            string content = await Utilities.GetHtmlAsync(url, UserAgentHeader);
            string fileName = "C:/Users/georg/OneDrive/Desktop/cars-bg-mobile-page.html";
            await File.WriteAllTextAsync(fileName, content, Encoding.UTF8);

            /*
            BaseAdvertScraper scraper = new MobileBgAdvertScraper(context);
            await scraper.ScrapeAllAdvertsAsync(async (model) =>
            {
                using var db = new ApplicationDbContext();
                var service = new AdvertsService(db);
                await service.AddOrUpdateAsync(model);
            });*/
        }
    }
}
