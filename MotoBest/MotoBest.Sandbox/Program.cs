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
    using System.Threading;

    public class Program
    {
        public static async Task Main()
        {
            /*Console.OutputEncoding = Encoding.UTF8;

            

            Console.WriteLine("Database created");

            var service = new AdvertsService(db);

            Task.Run(() =>
            {
                for (int i = 0; i < 60; i++)
                {
                    Thread.Sleep(2000);
                    Console.WriteLine(i);
                }
            });*/

            var db = new ApplicationDbContext();
            await db.Database.EnsureDeletedAsync();
            Console.WriteLine("Database deleted");
            await db.Database.EnsureCreatedAsync();
            Console.WriteLine("Database created");

            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);

            var carmarketScraper = new MobileBgAdvertScraper(context);

            await carmarketScraper.ScrapeAllAdvertsAsync(async model =>
            {
                using var dbContext = new ApplicationDbContext();
                var service = new AdvertsService(dbContext);
                await service.AddAdvertAsync(model);
            });
            
            /*new Thread(async () =>
            {
                var mobileScraper = new MobileBgAdvertScraper(context);

                mobileScraper.ScrapeAllAdvertsAsync(async model =>
                {
                    using var dbContext = new ApplicationDbContext();
                    var service = new AdvertsService(dbContext);
                    await service.AddAdvertAsync(model);
                    Console.WriteLine("mobile");
                });
            }).Start();

            Console.ReadLine();

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
