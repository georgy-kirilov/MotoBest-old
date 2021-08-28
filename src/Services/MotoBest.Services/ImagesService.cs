namespace MotoBest.Services
{
    using System.Linq;

    using MotoBest.Data;
    using MotoBest.Data.Models;
    using MotoBest.Services.Contracts;

    public class ImagesService : IImagesService
    {
        private readonly ApplicationDbContext dbContext;

        public ImagesService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Image GetOrCreate(string url, Advert advert)
        {
            return dbContext.Images.FirstOrDefault(image => image.Url == url && advert.Id == image.AdvertId) ?? new Image { Advert = advert, Url = url };
        }
    }
}
