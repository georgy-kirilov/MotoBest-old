namespace MotoBest.Services
{
    using System.Linq;

    using MotoBest.Data;
    using MotoBest.Models;
    using MotoBest.Scraping.Common;

    public class ModelFactory : IModelFactory
    {
        private readonly ApplicationDbContext dbContext;

        public ModelFactory(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public AdvertProvider GetOrCreateAdvertProvider(AdvertScrapeModel scrapeModel)
        {
            return dbContext.AdvertProviders.FirstOrDefault(ap => ap.Name == scrapeModel.AdvertProviderName) ?? new AdvertProvider { Name = scrapeModel.AdvertProviderName, AdvertUrlFormat = scrapeModel.AdvertUrlFormat };
        }

        public Brand GetOrCreateBrand(string brandName)
        {
            if (brandName == null)
            {
                return null;
            }

            return dbContext.Brands.FirstOrDefault(brand => brand.Name == brandName) ?? new Brand { Name = brandName };
        }

        public Model GetOrCreateModel(string modelName, Brand brand)
        {
            if (modelName == null)
            {
                return null;
            }

            return brand.Models.FirstOrDefault(model => model.Name == modelName) ?? new Model { Name = modelName, Brand = brand };
        }

        public Color GetOrCreateColor(string colorName)
        {
            if (colorName == null)
            {
                return null;
            }

            return dbContext.Colors.FirstOrDefault(color => color.Name == colorName) ?? new Color { Name = colorName };
        }

        public Engine GetOrCreateEngine(string engineType)
        {
            return dbContext.Engines.FirstOrDefault(engine => engine.Type == engineType) ?? new Engine { Type = engineType };
        }

        public Transmission GetOrCreateTransmission(string transmissionType)
        {
            return dbContext.Transmissions.FirstOrDefault(transmission => transmission.Type == transmissionType) ?? new Transmission { Type = transmissionType };
        }

        public BodyStyle GetOrCreateBodyStyle(string bodyStyleName)
        {
            return dbContext.BodyStyles.FirstOrDefault(bodyStyle => bodyStyle.Name == bodyStyleName) ?? new BodyStyle { Name = bodyStyleName };
        }

        public Region GetOrCreateRegion(string regionName)
        {
            return dbContext.Regions.FirstOrDefault(region => region.Name == regionName) ?? new Region { Name = regionName };
        }

        public Town GetOrCreateTown(string townName, Region region)
        {
            if (townName == null)
            {
                return null;
            }

            return dbContext.Towns.FirstOrDefault(town => town.Name == townName) ?? new Town { Name = townName, Region = region };
        }

        public EuroStandard GetOrCreateEuroStandard(string euroStandardType)
        {
            if (euroStandardType == null)
            {
                return null;
            }

            return dbContext.EuroStandards.FirstOrDefault(euroStandard => euroStandard.Type == euroStandardType) ?? new EuroStandard { Type = euroStandardType };
        }

        public Image GetOrCreateImage(string url, Advert advert)
        {
            return dbContext.Images.FirstOrDefault(image => image.Url == url && advert.Id == image.AdvertId) ?? new Image { Advert = advert, Url = url };
        }

        public Condition GetOrCreateCondition(string conditionType)
        {
            return dbContext.Conditions.FirstOrDefault(condition => condition.Type == conditionType) ?? new Condition { Type = conditionType };
        }
    }
}
