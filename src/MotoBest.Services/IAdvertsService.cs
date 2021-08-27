namespace MotoBest.Services
{
    using System.Threading.Tasks;

    using MotoBest.Models;
    using MotoBest.Web.ViewModels;
    using MotoBest.Scraping.Common;
    using System.Collections.Generic;
    using MotoBest.Web.InputModels;

    public interface IAdvertsService
    {
        Task AddOrUpdateAdvertAsync(AdvertScrapeModel model);

        Advert GetAdvertById(string id);

        AdvertViewModel MapViewModelFrom(Advert advert);

        IEnumerable<AdvertViewModel> GetLatestAdverts(int pageIndex);

        IEnumerable<AdvertViewModel> SearchForAdverts(SearchAdvertsInputModel input);

        SearchAdvertsViewModel CreateSearchAdvertsViewModel();

        int GetAllAdvertsCount();
    }
}
