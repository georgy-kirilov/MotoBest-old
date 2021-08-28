namespace MotoBest.Services.Contracts
{
    using MotoBest.Data.Models;

    public interface IImagesService
    {
        Image GetOrCreate(string url, Advert advert);
    }
}
