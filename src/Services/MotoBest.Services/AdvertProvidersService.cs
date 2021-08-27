namespace MotoBest.Services
{
    using System.Linq;

    using MotoBest.Data;
    using MotoBest.Models;
    using MotoBest.Scraping.Common;
    using MotoBest.Services.Contracts;

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
