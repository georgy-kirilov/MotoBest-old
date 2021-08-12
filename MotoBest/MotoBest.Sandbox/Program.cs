namespace MotoBest.Sandbox
{
    using System;
    using Scraper;
    using AngleSharp;
    using AngleSharp.Dom;
    using System.Threading.Tasks;
    using MotoBest.Data;
    using MotoBest.Services;
    using Models;
    using System.Linq;
    using System.IO;
    using System.Collections.Generic;
    using System.Text.Json;
    using System.Diagnostics;
    using System.Text;

    public class Program
    {
        public static async Task Main()
        {
            /*Console.OutputEncoding = Encoding.UTF8;

            var db = new ApplicationDbContext();
            await db.Database.EnsureDeletedAsync();
            await db.Database.EnsureCreatedAsync();

            Console.WriteLine("Database created");

            var service = new AdvertsService(db);*/
            
            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);

            var scraper = new MobileBgAdvertScraper(context);
            var advert = await scraper.ScrapeAdvertAsync("11622188754581401");

            /*var exceptions = new List<Exception>();

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            await scraper.ScrapeAllAdvertsAsync(async model =>
            {
                using var dbContext = new ApplicationDbContext();
                service = new AdvertsService(dbContext);
                await service.AddAdvertAsync(model);
            });

            stopwatch.Stop();
            Console.WriteLine(stopwatch.Elapsed);

            string path = @"C:\Users\georg\OneDrive\Desktop\exceptions.txt";
            string json = JsonSerializer.Serialize(exceptions);
            await File.WriteAllTextAsync(path, json);*/
        }
    }
}
