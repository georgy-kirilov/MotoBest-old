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
    using System.Linq;
    using MotoBest.Models;

    public class Program
    {
        public static async Task Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            
            //await new ApplicationDbContext().Database.EnsureDeletedAsync();
            Console.WriteLine("Database deleted");

            await new ApplicationDbContext().Database.EnsureCreatedAsync();
            Console.WriteLine("Database created");

            var appSeeder = new ApplicationSeeder();
            await appSeeder.SeedAsync(new ApplicationDbContext());

            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);

            BaseWebScraper scraper = new MobileBgWebScraper(context);
            
            await scraper.ScrapeAllAdvertsAsync(async (model) =>
            {
                Console.WriteLine(model);
                using var db = new ApplicationDbContext();
                var service = new AdvertsService(db);
                await service.AddOrUpdateAsync(model);
            });
        }
    }
}
