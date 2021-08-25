namespace MotoBest.Web.Controllers
{
    using System.Linq;
    using System.Diagnostics;

    using Microsoft.AspNetCore.Mvc;

    using MotoBest.Data;
    using MotoBest.Web.Models;
    
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
    }
}
