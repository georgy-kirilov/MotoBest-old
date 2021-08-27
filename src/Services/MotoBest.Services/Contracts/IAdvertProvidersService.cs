namespace MotoBest.Services.Contracts
{
    using MotoBest.Models;
    using MotoBest.Scraping.Common;

    public interface IAdvertProvidersService
    {
        AdvertProvider GetOrCreate(AdvertScrapeModel scrapeModel);
    }
}
