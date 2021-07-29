namespace MotoBest.Services
{
    using Data;
    using Models;
    using Scraper;

    using System;
    using System.Linq;
    using System.Threading.Tasks;

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

            Brand brand = modelFactory.CreateBrand(scrapeModel);
            Model model = modelFactory.CreateModel(scrapeModel, brand);
            Color color = modelFactory.CreateColor(scrapeModel);
            Engine engine = modelFactory.CreateEngine(scrapeModel);
            Transmission transmission = modelFactory.CreateTransmission(scrapeModel);
            BodyStyle bodyStyle = modelFactory.CreateBodyStyle(scrapeModel);
            Region region = modelFactory.CreateRegion(scrapeModel);
            Town town = modelFactory.CreateTown(scrapeModel, region);
            EuroStandard euroStandard = modelFactory.CreateEuroStandard(scrapeModel);

            advert.AdvertProvider = advertProvider;
            advert.Brand = brand;
            advert.Model = model;
            advert.Color = color;
            advert.Engine = engine;
            advert.Transmission = transmission;
            advert.BodyStyle = bodyStyle;
            advert.Region = region;
            advert.Town = town;
            advert.EuroStandard = euroStandard;
            advert.Views = scrapeModel.Views;
            advert.Kilometrage = scrapeModel.Kilometrage;
            advert.HorsePowers = scrapeModel.HorsePowers;
            advert.Title = scrapeModel.Title;
            advert.Description = scrapeModel.Description;
            advert.Price = scrapeModel.Price;
            advert.ManufacturingDate = scrapeModel.ManufacturingDate;

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
