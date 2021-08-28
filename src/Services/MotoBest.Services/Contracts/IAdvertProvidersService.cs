namespace MotoBest.Services.Contracts
{
    using MotoBest.Data.Models;
    using MotoBest.Data.Scraping.Common;

    public interface IAdvertProvidersService
    {
        AdvertProvider GetOrCreate(AdvertScrapeModel scrapeModel);
    }
}
