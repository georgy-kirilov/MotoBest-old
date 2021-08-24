namespace MotoBest.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using MotoBest.Data;
    using MotoBest.Models;
    using MotoBest.Services;
    using MotoBest.Web.ViewModels;

    public class AdvertsController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IAdvertsService advertsService;

        public AdvertsController(ApplicationDbContext dbContext, IAdvertsService advertsService)
        {
            this.dbContext = dbContext;
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
        public IActionResult Latest(int pageIndex)
        {
            var adverts = advertsService.GetLatestAdverts(pageIndex);

            if (adverts == null)
            {
                return NotFound();
            }
            
            return View(adverts);
        }
    }
}
