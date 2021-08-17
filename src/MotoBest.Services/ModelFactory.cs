namespace MotoBest.Services
{
    using System.Linq;

    using MotoBest.Data;
    using MotoBest.Models;
    using MotoBest.Scraping.Common;

    internal class ModelFactory
    {
        private readonly ApplicationDbContext dbContext;

        public ModelFactory(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public AdvertProvider CreateAdvertProvider(AdvertScrapeModel scrapeModel)
        {
            return dbContext.AdvertProviders.FirstOrDefault(ap => ap.Name == scrapeModel.AdvertProviderName)
                ?? new AdvertProvider { Name = scrapeModel.AdvertProviderName, AdvertUrlFormat = scrapeModel.AdvertUrlFormat };
        }

        public Brand CreateBrand(AdvertScrapeModel scrapeModel)
        {
            if (scrapeModel.BrandName == null)
            {
                return null;
            }

            return dbContext.Brands.FirstOrDefault(b => b.Name == scrapeModel.BrandName)
                ?? new Brand { Name = scrapeModel.BrandName };
        }

        public Model CreateModel(AdvertScrapeModel scrapeModel, Brand brand)
        {
            if (scrapeModel.ModelName == null)
            {
                return null;
            }

            return brand.Models.FirstOrDefault(m => m.Name == scrapeModel.ModelName)
                ?? new Model { Name = scrapeModel.ModelName, Brand = brand };
        }

        public Color CreateColor(AdvertScrapeModel scrapeModel)
        {
            if (scrapeModel.ColorName == null)
            {
                return null;
            }

            return dbContext.Colors.FirstOrDefault(c => c.Name == scrapeModel.ColorName) 
                ?? new Color { Name = scrapeModel.ColorName };
        }

        public Engine CreateEngine(AdvertScrapeModel scrapeModel)
        {
            return dbContext.Engines.FirstOrDefault(e => e.Type == scrapeModel.EngineType)
                ?? new Engine { Type = scrapeModel.EngineType };
        }

        public Transmission CreateTransmission(AdvertScrapeModel scrapeModel)
        {
            return dbContext.Transmissions.FirstOrDefault(t => t.Type == scrapeModel.TransmissionType)
                ?? new Transmission { Type = scrapeModel.TransmissionType };
        }

        public BodyStyle CreateBodyStyle(AdvertScrapeModel scrapeModel)
        {
            return dbContext.BodyStyles.FirstOrDefault(bs => bs.Name == scrapeModel.BodyStyleName)
                ?? new BodyStyle { Name = scrapeModel.BodyStyleName };
        }

        public Region CreateRegion(AdvertScrapeModel scrapeModel)
        {
            return dbContext.Regions.FirstOrDefault(r => r.Name == scrapeModel.RegionName)
                ?? new Region { Name = scrapeModel.RegionName };
        }

        public Town CreateTown(AdvertScrapeModel scrapeModel, Region region)
        {
            if (scrapeModel.TownName == null)
            {
                return null;
            }

            return dbContext.Towns.FirstOrDefault(t => t.Name == scrapeModel.TownName)
                ?? new Town { Name = scrapeModel.TownName, Region = region };
        }

        public EuroStandard CreateEuroStandard(AdvertScrapeModel scrapeModel)
        {
            if (scrapeModel.EuroStandardType == null)
            {
                return null;
            }

            return dbContext.EuroStandards.FirstOrDefault(es => es.Type == scrapeModel.EuroStandardType) 
                ?? new EuroStandard { Type = scrapeModel.EuroStandardType };
        }

        public Image CreateImage(string url, Advert advert)
        {
            return dbContext.Images.FirstOrDefault(i => i.Url == url && advert.Id == i.AdvertId) 
                ?? new Image { Advert = advert, Url = url };
        }

        public Condition CreateCondition(AdvertScrapeModel scrapeModel)
        {
            return dbContext.Conditions.FirstOrDefault(c => c.Type == scrapeModel.Condition) 
                ?? new Condition { Type = scrapeModel.Condition };
        }
    }
}
