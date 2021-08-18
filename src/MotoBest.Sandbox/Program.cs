﻿namespace MotoBest.Sandbox
{
    using System;
    using System.Threading.Tasks;

    using AngleSharp;

    using MotoBest.Data;
    using MotoBest.Services;
    using MotoBest.Seeding.Seeders;
    using MotoBest.Scraping.Scrapers;
    using System.Text;

    public class Program
    {
        public static async Task Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            await new ApplicationDbContext().Database.EnsureDeletedAsync();
            Console.WriteLine("Database deleted");

            await new ApplicationDbContext().Database.EnsureCreatedAsync();
            Console.WriteLine("Database created");

            var appSeeder = new ApplicationSeeder();
            await appSeeder.SeedAsync(new ApplicationDbContext());

            Console.WriteLine("Seeding complete. Press any key to continue...");
            Console.ReadKey();
            Console.WriteLine("Scraping...");

            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);

            BaseAdvertScraper scraper = new CarmarketAdvertScraper(context);

            await scraper.ScrapeAllAdvertsAsync(async (model) =>
            {
                using var db = new ApplicationDbContext();
                var service = new AdvertsService(db);
                await service.AddOrUpdateAsync(model);
            });
        }
    }
}