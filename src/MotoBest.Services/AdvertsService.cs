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

        public async Task AddAdvertAsync(AdvertScrapeModel scrapeModel)
        {
            if (scrapeModel == null)
            {
                throw new ArgumentNullException(nameof(scrapeModel));
            }

            AdvertProvider advertProvider = modelFactory.CreateAdvertProvider(scrapeModel);

            Advert advert = advertProvider.Adverts.FirstOrDefault(a => a.RemoteId == scrapeModel.RemoteId);
            bool isAdvertNew = false;

            if (advert == null)
            {
                advert = new Advert { RemoteId = scrapeModel.RemoteId };
                isAdvertNew = true;
            }

            advert.AdvertProvider = advertProvider;
            advert.Brand = modelFactory.CreateBrand(scrapeModel);
            advert.Model = modelFactory.CreateModel(scrapeModel, advert.Brand);
            advert.Color = modelFactory.CreateColor(scrapeModel);
            advert.Engine = modelFactory.CreateEngine(scrapeModel);
            advert.Transmission = modelFactory.CreateTransmission(scrapeModel);
            advert.BodyStyle = modelFactory.CreateBodyStyle(scrapeModel);
            advert.Region = modelFactory.CreateRegion(scrapeModel);
            advert.Town = modelFactory.CreateTown(scrapeModel, advert.Region);
            advert.EuroStandard = modelFactory.CreateEuroStandard(scrapeModel);
            advert.Condition = modelFactory.CreateCondition(scrapeModel);

            advert.Views = scrapeModel.Views;
            advert.Kilometrage = scrapeModel.Kilometrage;
            advert.HorsePowers = scrapeModel.HorsePowers;
            advert.Title = scrapeModel.Title;
            advert.Description = scrapeModel.Description;
            advert.Price = scrapeModel.Price;
            advert.ManufacturingDate = scrapeModel.ManufacturingDate;
            advert.LastModifiedOn = scrapeModel.LastModifiedOn;
            advert.IsNewImport = scrapeModel.IsNewImport;
            advert.HasFourDoors = scrapeModel.HasFourDoors;
            advert.IsEuroStandardExact = scrapeModel.IsEuroStandardExact;

            foreach (string imageUrl in scrapeModel.ImageUrls)
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
