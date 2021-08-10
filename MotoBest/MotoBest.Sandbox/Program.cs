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

    public class Program
    {
        public static async Task Main()
        {
            var db = new ApplicationDbContext();
            await db.Database.EnsureDeletedAsync();
            await db.Database.EnsureCreatedAsync();

            Console.WriteLine("Database created");

            var service = new AdvertsService(db);

            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);

            var scraper = new CarmarketBgAdvertScraper(context);
            var exceptions = new List<Exception>();

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            await scraper.ScrapeAllAdvertsAsync(async (model) =>
            {
                using var dbContext = new ApplicationDbContext();
                service = new AdvertsService(dbContext);
                await service.AddAdvertAsync(model);
            });

            stopwatch.Stop();
            Console.WriteLine(stopwatch.Elapsed);

            string path = @"C:\Users\georg\OneDrive\Desktop\exceptions.txt";
            string json = JsonSerializer.Serialize(exceptions);
            await File.WriteAllTextAsync(path, json);
        }

        public static async Task<HashSet<string>> GetMobileBgColorsAsync(IBrowsingContext context)
        {
            string text = await File.ReadAllTextAsync("./Resources/mobile-colors.html");
            var dom = await context.OpenAsync(x => x.Content(text));

            return dom.QuerySelectorAll("select > option").Select(o => o.GetAttribute("value").Trim().ToLower()).ToHashSet();
        }

        public static async Task<HashSet<string>> GetCarsBgColorsAsync(IBrowsingContext context)
        {
            string text = await File.ReadAllTextAsync("./Resources/cars.bg-colors.html");
            var dom = await context.OpenAsync(x => x.Content(text));

            return dom.QuerySelectorAll("div.mdc-chip-set mdc-chip-set--choice > label")
               .Select(l => l.QuerySelector("span > label").TextContent.Trim().ToLower())
               .ToHashSet();
        }

        public static async Task<HashSet<string>> GetCarmarketColorsAsync(IBrowsingContext context)
        {
            string text = await File.ReadAllTextAsync("./Resources/carmarket-colors.html");
            var dom = await context.OpenAsync(x => x.Content(text));

            return dom.QuerySelectorAll("select > option").Select(o => o.TextContent.Trim().ToLower()).ToHashSet();
        }

        public static async Task OldStuff()
        {
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
