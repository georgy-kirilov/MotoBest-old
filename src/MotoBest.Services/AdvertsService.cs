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

        public async Task AddAdvertAsync(AdvertScrapeModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            AdvertProvider advertProvider = modelFactory.CreateAdvertProvider(model);

            Advert advert = advertProvider.Adverts.FirstOrDefault(a => a.RemoteId == model.RemoteId);
            bool isAdvertNew = false;

            if (advert == null)
            {
                advert = new Advert { RemoteId = model.RemoteId };
                isAdvertNew = true;
            }

            advert.AdvertProvider = advertProvider;
            advert.Brand = modelFactory.CreateBrand(model.BrandName);
            advert.Model = modelFactory.CreateModel(model.ModelName, advert.Brand);
            advert.Color = modelFactory.CreateColor(model.ColorName);
            advert.Engine = modelFactory.CreateEngine(model.EngineType);
            advert.Transmission = modelFactory.CreateTransmission(model.TransmissionType);
            advert.BodyStyle = modelFactory.CreateBodyStyle(model.BodyStyleName);
            advert.Region = modelFactory.CreateRegion(model.RegionName);
            advert.Town = modelFactory.CreateTown(model, advert.Region);
            advert.EuroStandard = modelFactory.CreateEuroStandard(model.EuroStandardType);
            advert.Condition = modelFactory.CreateCondition(model.Condition);

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
                Image image = modelFactory.CreateImage(imageUrl, advert);
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
