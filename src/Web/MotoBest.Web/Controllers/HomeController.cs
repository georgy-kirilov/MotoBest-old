namespace MotoBest.Web.Controllers
{
    using System.Diagnostics;

    using Microsoft.AspNetCore.Mvc;

    using MotoBest.Services;
    using MotoBest.Web.Models;
    using MotoBest.Web.ViewModels;
    using MotoBest.Web.InputModels;
    using MotoBest.Web.CombinedModels;

    public class HomeController : Controller
    {
        private readonly IAdvertsService advertsService;

        public HomeController(IAdvertsService advertsService)
        {
            this.advertsService = advertsService;
        }

        public IActionResult Index()
        {
            return View(advertsService.GetAllAdvertsCount());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult Search()
        {
            var combinedModel = new SearchAdvertsCombinedModel
            {
                View = advertsService.CreateSearchAdvertsViewModel(),
                Input = new SearchAdvertsInputModel(),
            };

            return View(combinedModel);
        }

        [HttpGet]
        public IActionResult StartSearch(SearchAdvertsCombinedModel model)
        {
            return View("SearchingResults", new SearchResultsViewModel { Adverts = advertsService.SearchForAdverts(model.Input) });
        }
    }
}
