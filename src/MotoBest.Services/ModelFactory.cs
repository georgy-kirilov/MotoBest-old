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

        public Brand CreateBrand(string brand)
        {
            if (brand == null)
            {
                return null;
            }

            return dbContext.Brands.FirstOrDefault(b => b.Name == brand) ?? new Brand { Name = brand };
        }

        public Model CreateModel(string model, Brand brand)
        {
            if (model == null)
            {
                return null;
            }

            return brand.Models.FirstOrDefault(m => m.Name == model) ?? new Model { Name = model, Brand = brand };
        }

        public Color CreateColor(string color)
        {
            if (color == null)
            {
                return null;
            }

            return dbContext.Colors.FirstOrDefault(c => c.Name == color) ?? new Color { Name = color };
        }

        public Engine CreateEngine(string engine)
        {
            return dbContext.Engines.FirstOrDefault(e => e.Type == engine) ?? new Engine { Type = engine };
        }

        public Transmission CreateTransmission(string transmission)
        {
            return dbContext.Transmissions.FirstOrDefault(t => t.Type == transmission) ?? new Transmission { Type = transmission };
        }

        public BodyStyle CreateBodyStyle(string bodyStyle)
        {
            return dbContext.BodyStyles.FirstOrDefault(bs => bs.Name == bodyStyle) ?? new BodyStyle { Name = bodyStyle };
        }

        public Region CreateRegion(string region)
        {
            return dbContext.Regions.FirstOrDefault(r => r.Name == region) ?? new Region { Name = region };
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

        public EuroStandard CreateEuroStandard(string euroStandard)
        {
            if (euroStandard == null)
            {
                return null;
            }

            return dbContext.EuroStandards.FirstOrDefault(es => es.Type == euroStandard) ?? new EuroStandard { Type = euroStandard };
        }

        public Image CreateImage(string url, Advert advert)
        {
            return dbContext.Images.FirstOrDefault(i => i.Url == url && advert.Id == i.AdvertId) 
                ?? new Image { Advert = advert, Url = url };
        }

        public Condition CreateCondition(string condition)
        {
            return dbContext.Conditions.FirstOrDefault(c => c.Type == condition) ?? new Condition { Type = condition };
        }
    }
}
