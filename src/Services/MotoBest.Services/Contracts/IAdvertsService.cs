namespace MotoBest.Services.Contracts
{
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using MotoBest.Models;
    using MotoBest.Web.ViewModels;
    using MotoBest.Web.InputModels;
    using MotoBest.Data.Scraping.Common;

    public interface IAdvertsService
    {
        Task AddOrUpdateAdvertAsync(AdvertScrapeModel model);

        Advert GetAdvertById(string id);

        AdvertViewModel MapViewModelFrom(Advert advert);

        IEnumerable<AdvertViewModel> GetLatestAdverts(int pageIndex);

        IEnumerable<AdvertViewModel> SearchAdverts(SearchAdvertsInputModel input);

        SearchAdvertsViewModel CreateSearchAdvertsViewModel();

        int GetAllAdvertsCount();
    }
}
