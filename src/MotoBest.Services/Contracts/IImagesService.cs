namespace MotoBest.Services.Contracts
{
    using MotoBest.Models;

    public interface IImagesService
    {
        Image GetOrCreate(string url, Advert advert);
    }
}
