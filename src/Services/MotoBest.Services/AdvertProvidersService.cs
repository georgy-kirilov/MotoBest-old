namespace MotoBest.Services
{
    using System.Linq;

    using MotoBest.Data;
    using MotoBest.Data.Models;
    using MotoBest.Services.Contracts;
    using MotoBest.Data.Scraping.Common;

    public class AdvertProvidersService : IAdvertProvidersService
    {
        private readonly ApplicationDbContext dbContext;

        public AdvertProvidersService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public AdvertProvider GetOrCreate(AdvertScrapeModel scrapeModel)
        {
            return dbContext.AdvertProviders.FirstOrDefault(ap => ap.Name == scrapeModel.AdvertProviderName) 
                ?? new AdvertProvider { Name = scrapeModel.AdvertProviderName, AdvertUrlFormat = scrapeModel.AdvertUrlFormat };
        }
    }
}
