namespace MotoBest.Scraping.Scrapers
{
    using System;
    using System.Threading.Tasks;

    using MotoBest.Scraping.Common;

    public interface IAdvertScraper
    {
        string AdvertUrlFormat { get; }

        Task<AdvertScrapeModel> ScrapeAdvertAsync(string remoteId);

        Task ScrapeLatestAdvertsAsync(Action<AdvertScrapeModel> action);

        Task ScrapeAllAdvertsAsync(Action<AdvertScrapeModel> action);
    }
}
