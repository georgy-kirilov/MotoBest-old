namespace MotoBest.RawDataNormalizer
{
    using MotoBest.Models;
    using MotoBest.Scraper;

    public interface IAdvertNormalizer
    {
        AdvertScrapeModel NormalizeAdvert(AdvertScrapeModel model);
    }
}
