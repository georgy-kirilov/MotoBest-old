namespace MotoBest.Scraper
{
    using System;
    using System.Threading.Tasks;

    using AngleSharp.Dom;

    public interface IWebScraper
    {
        Task<AdvertScrapeModel> ScrapeAdvertAsync(string remoteId);

        Task ScrapeLatestAdvertsAsync(Action<AdvertScrapeModel> action);

        Task ScrapeAllAdvertsAsync(string brandName, string modelName, Action<AdvertScrapeModel> action);
    }
}
