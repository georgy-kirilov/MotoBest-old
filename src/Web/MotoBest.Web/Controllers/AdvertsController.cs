namespace MotoBest.Web.Controllers
{
    using System.Linq;
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Mvc;

    using MotoBest.Data;
    using MotoBest.Models;
    using MotoBest.Services;
    using MotoBest.Web.ViewModels;
    using MotoBest.Web.InputModels;

    public class AdvertsController : Controller
    {
        private readonly IAdvertsService advertsService;
        private readonly ApplicationDbContext dbContext;

        public AdvertsController(IAdvertsService advertsService, ApplicationDbContext dbContext)
        {
            this.advertsService = advertsService;
            this.dbContext = dbContext;
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

        [HttpPost]
        public ActionResult<IEnumerable<string>> GetModelsByBrand([FromBody] GetModelsByBrandInputModel input)
        {
            return dbContext.Models.Where(model => model.Brand.Name == input.Brand)
                                         .OrderBy(model => model.Name)
                                         .Select(model => model.Name)
                                         .ToList();
        }

        [HttpPost]
        public ActionResult<IEnumerable<string>> GetTownsByRegion([FromBody] GetTownsByRegionInputModel input)
        {
            return dbContext.Towns.Where(town => town.Region.Name == input.Region)
                                       .OrderBy(town => town.Name)
                                       .Select(town => town.Name)
                                       .ToList();
        }
    }
}
