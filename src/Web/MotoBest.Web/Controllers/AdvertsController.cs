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

            AdvertViewModel viewModel = advertsService.MapViewModelFrom(advert);
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Search(IEnumerable<AdvertViewModel> adverts)
        {
            var viewModel = new SearchResultsViewModel
            {
                Adverts = adverts
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Latest(int id)
        {
            var adverts = advertsService.GetLatestAdverts(id);

            var viewModel = new SearchResultsViewModel
            {
                Adverts = adverts,
                PageIndex = id,
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult<IEnumerable<string>> GetModelsByBrand([FromBody] GetModelsByBrandInputModel input)
        {
            var models = modelsService.GetAllModelNamesByBrandName(input.Brand);
            return Ok(models);
        }

        [HttpPost]
        public ActionResult<IEnumerable<string>> GetTownsByRegion([FromBody] GetTownsByRegionInputModel input)
        {
            var towns = townsService.GetAllTownNamesByRegionName(input.Region);
            return Ok(towns);
        }
    }
}
