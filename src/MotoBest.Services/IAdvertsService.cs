namespace MotoBest.Services
{
    using System.Threading.Tasks;

    using MotoBest.Scraping.Common;

    public interface IAdvertsService
    {
        Task AddOrUpdateAsync(AdvertScrapeModel model);
    }
}
