namespace MotoBest.Services
{
    using Scraper;
    using System.Threading.Tasks;

    public interface IAdvertsService
    {
        Task AddAdvertAsync(AdvertScrapeModel scrapeModel);
    }
}
