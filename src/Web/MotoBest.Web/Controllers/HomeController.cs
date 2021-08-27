namespace MotoBest.Web.Controllers
{
    using System.Linq;
    using System.Diagnostics;
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Mvc;

    using MotoBest.Data;
    using MotoBest.Web.Models;
    using MotoBest.Web.ViewModels;
    using MotoBest.Common;

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
                KeyValuePairs = new List<KeyValuePair<string, IEnumerable<string>>>()
                {
                    new("Марка", dbContext.Brands.Select(brand => brand.Name.Capitalize())),
                    new("Модел", new List<string>()),
                    new("Двигател", dbContext.Engines.Select(engine => engine.Type.Capitalize())),
                    new("Евро стандарт", dbContext.EuroStandards.Select(euroStandard => euroStandard.Type.Capitalize())),
                    new("Тип", dbContext.BodyStyles.Select(bodyStyle => bodyStyle.Name.Capitalize())),
                    new("Скоростна кутия", dbContext.Transmissions.Select(transmission => transmission.Type.Capitalize())),
                    new("Цвят", dbContext.Colors.Select(color => color.Name.Capitalize())),
                    new("Регион", dbContext.Regions.Select(region => region.Name.Capitalize())),
                    new("Състояние", dbContext.Conditions.Select(condition => condition.Type.Capitalize())),
                }
            };

            return View(viewModel);
        }
    }
}
