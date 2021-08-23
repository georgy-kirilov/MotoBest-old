namespace MotoBest.Services
{
    using System.Threading.Tasks;
    using MotoBest.Models;
    using MotoBest.Scraping.Common;

    public interface IAdvertsService
    {
        Task AddOrUpdateAsync(AdvertScrapeModel model);

        Advert GetAdvertById(string id);
    }
}
