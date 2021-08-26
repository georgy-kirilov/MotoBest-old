namespace MotoBest.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using MotoBest.Models;
    using MotoBest.Services;
    using MotoBest.Web.ViewModels;

    public class AdvertsController : Controller
    {
        private readonly IAdvertsService advertsService;

        public AdvertsController(IAdvertsService advertsService)
        {
            this.advertsService = advertsService;
        }

        [HttpGet]
        public IActionResult ById(string id)
        {
            Advert advert = advertsService.GetAdvertById(id);

            if (advert == null)
            {
                return NotFound();
            }

            AdvertViewModel viewModel = advertsService.MapViewModelFrom(advert);
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Latest(int id)
        {
            var adverts = advertsService.GetLatestAdverts(id);

            var viewModel = new LatestAdvertsViewModel
            {
                Adverts = adverts,
                PageIndex = id,
            };

            return View(viewModel);
        }
    }
}
