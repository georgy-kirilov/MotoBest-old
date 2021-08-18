namespace MotoBest.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using MotoBest.Data;
    using MotoBest.Models;
    using MotoBest.Scraping.Common;

    public class AdvertsService : IAdvertsService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ModelFactory modelFactory;

        public AdvertsService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
            modelFactory = new ModelFactory(dbContext);
        }

        public async Task AddOrUpdateAsync(AdvertScrapeModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            AdvertProvider advertProvider = modelFactory.GetOrCreateAdvertProvider(model);

            Advert advert = advertProvider.Adverts.FirstOrDefault(a => a.RemoteId == model.RemoteId);
            bool isAdvertNew = false;

            if (advert == null)
            {
                advert = new Advert { RemoteId = model.RemoteId };
                isAdvertNew = true;
            }

            advert.AdvertProvider = advertProvider;
            advert.Brand = modelFactory.GetOrCreateBrand(model.BrandName);
            advert.Model = modelFactory.GetOrCreateModel(model.ModelName, advert.Brand);
            advert.Color = modelFactory.GetOrCreateColor(model.ColorName);
            advert.Engine = modelFactory.GetOrCreateEngine(model.EngineType);
            advert.Transmission = modelFactory.GetOrCreateTransmission(model.TransmissionType);
            advert.BodyStyle = modelFactory.GetOrCreateBodyStyle(model.BodyStyleName);
            advert.Region = modelFactory.GetOrCreateRegion(model.RegionName);
            advert.Town = modelFactory.GetOrCreateTown(model.TownName, advert.Region);
            advert.EuroStandard = modelFactory.GetOrCreateEuroStandard(model.EuroStandardType);
            advert.Condition = modelFactory.GetOrCreateCondition(model.Condition);

            advert.Views = model.Views;
            advert.Kilometrage = model.Kilometrage;
            advert.HorsePowers = model.HorsePowers;
            advert.Title = model.Title;
            advert.Description = model.Description;
            advert.Price = model.Price;
            advert.ManufacturingDate = model.ManufacturingDate;
            advert.LastModifiedOn = model.LastModifiedOn;
            advert.IsNewImport = model.IsNewImport;
            advert.HasFourDoors = model.HasFourDoors;
            advert.IsEuroStandardExact = model.IsEuroStandardExact;

            foreach (string imageUrl in model.ImageUrls)
            {
                Image image = modelFactory.GetOrCreateImage(imageUrl, advert);
                advert.Images.Add(image);
            }

            if (isAdvertNew)
            {
                await dbContext.Adverts.AddAsync(advert);
            }

            await dbContext.SaveChangesAsync();
        }
    }
}
