namespace MotoBest.Web.Controllers
{
    using System.Linq;
    using System.Diagnostics;
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Mvc;

    using MotoBest.Data;
    using MotoBest.Web.Models;
    using MotoBest.Models.Common;
    using MotoBest.Web.ViewModels;
    using MotoBest.Web.InputModels;
    using MotoBest.Web.CombinedModels;

    public class HomeController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public HomeController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IActionResult Index()
        {
            int totalAvdertsCount = dbContext.Adverts.Count();
            return View(totalAvdertsCount);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult Search()
        {
            var viewModel = new SearchAdvertsViewModel
            {
                Brands = SelectNameableModels(dbContext.Brands),
                Engines = SelectTypeableModels(dbContext.Engines),
                Transmissions = SelectTypeableModels(dbContext.Transmissions),
                BodyStyles = SelectNameableModels(dbContext.BodyStyles),
                Conditions = SelectTypeableModels(dbContext.Conditions),
                Colors = SelectNameableModels(dbContext.Colors),
                EuroStandards = SelectTypeableModels(dbContext.EuroStandards),
                Regions = SelectNameableModels(dbContext.Regions),
            };

            return View(new SearchAdvertsCombinedModel
            {
                View = viewModel,
                Input = new SearchAdvertsInputModel(),
            });
        }

        public IEnumerable<string> SelectNameableModels<T>(IQueryable<T> queryable) where T : BaseNameableModel
        {
            return queryable.OrderBy(model => model.Name).Select(model => model.Name);
        }

        public IEnumerable<string> SelectTypeableModels<T>(IQueryable<T> queryable) where T : BaseTypeableModel
        {
            return queryable.OrderBy(model => model.Type).Select(model => model.Type);
        }
    }
}
