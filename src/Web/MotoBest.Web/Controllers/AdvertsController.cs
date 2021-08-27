namespace MotoBest.Web.Controllers
{
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Mvc;

    using MotoBest.Models;
    using MotoBest.Web.ViewModels;
    using MotoBest.Web.InputModels;
    using MotoBest.Services.Contracts;
    using System.Linq;

    public class AdvertsController : Controller
    {
        private readonly IAdvertsService advertsService;
        private readonly IModelsService modelsService;
        private readonly ITownsService townsService;

        public AdvertsController(IAdvertsService advertsService, IModelsService modelsService, ITownsService townsService)
        {
            this.advertsService = advertsService;
            this.modelsService = modelsService;
            this.townsService = townsService;
        }

        [HttpGet]
        public IActionResult ById(string id)
        {
            Advert advert = advertsService.GetAdvertById(id);

            if (advert == null)
            {
                return NotFound();
            }

            var viewModel = advertsService.MapViewModelFrom(advert);
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Search(SearchAdvertsInputModel input)
        {
            var viewModel = new SearchAdvertsResultsViewModel
            {
                Adverts = advertsService.SearchAdverts(input)
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Latest(int id)
        {
            var adverts = advertsService.GetLatestAdverts(id);

            var viewModel = new SearchAdvertsResultsViewModel
            {
                Adverts = adverts
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult<IEnumerable<string>> GetModelsByBrand([FromBody] GetModelsByBrandInputModel input)
        {
            var models = modelsService.GetAllModelsByBrandId(input.BrandId);
            return Ok(models);
        }

        [HttpPost]
        public ActionResult<IEnumerable<string>> GetTownsByRegion([FromBody] GetTownsByRegionInputModel input)
        {
            var towns = townsService.GetAllTownsByRegionId(input.RegionId);
            return Ok(towns);
        }
    }
}
