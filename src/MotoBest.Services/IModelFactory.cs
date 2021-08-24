namespace MotoBest.Services
{
    using MotoBest.Models;
    using MotoBest.Scraping.Common;

    public interface IModelFactory
    {
        AdvertProvider GetOrCreateAdvertProvider(AdvertScrapeModel scrapeModel);

        Brand GetOrCreateBrand(string brandName);

        Model GetOrCreateModel(string modelName, Brand brand);

        Color GetOrCreateColor(string colorName);

        Engine GetOrCreateEngine(string engineType);

        Transmission GetOrCreateTransmission(string transmissionType);

        BodyStyle GetOrCreateBodyStyle(string bodyStyleName);

        Region GetOrCreateRegion(string regionName);

        Town GetOrCreateTown(string townName, Region region);

        EuroStandard GetOrCreateEuroStandard(string euroStandardType);

        Image GetOrCreateImage(string url, Advert advert);

        Condition GetOrCreateCondition(string conditionType);
    }
}
