namespace MotoBest.Web.Controllers
{
    using System.Linq;
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Mvc;

    using MotoBest.Models;
    using MotoBest.Common;
    using MotoBest.Services;
    using MotoBest.Web.ViewModels;

    public class AdvertsController : Controller
    {
        private readonly IAdvertsService advertsService;
        private readonly IAdvertsFormatter advertsFormatter;

        public AdvertsController(IAdvertsService advertsService, IAdvertsFormatter advertsFormatter)
        {
            this.advertsService = advertsService;
            this.advertsFormatter = advertsFormatter;
        }

        [HttpGet]
        public IActionResult ById(string id)
        {
            Advert advert = advertsService.GetAdvertById(id);

            if (advert == null)
            {
                return NotFound();
            }

            var viewModel = new AdvertViewModel
            {
                Title = advert.Title,
                Description = advert.Description,
                Price = advert.Price,
                ImageUrls = advert.Images.Select(i => i.Url),
            };

            var infoPairRows = new List<KeyValuePair<string, string>>
            {
                new("Марка", advert.Brand?.Name),
                new("Модел", advert.Model?.Name),
                new("Дата на производство", advertsFormatter.FormatManufacturingDate(advert.ManufacturingDate)),
                new("Скоростна кутия", advert.Transmission?.Type.Capitalize()),
                new("Двигател", advert.Engine?.Type.Capitalize()),
                new("Тип", advert.BodyStyle?.Name.Capitalize()),
                new("Евро стандарт", advertsFormatter.FormatEuroStandard(advert.EuroStandard?.Type, advert.IsEuroStandardExact)),
                new("Мощност",  advertsFormatter.FormatHorsePowers(advert.HorsePowers)),
                new("Пробег", advertsFormatter.FormatKilometrage(advert.Kilometrage)),
                new("Цвят", advert.Color?.Name.Capitalize()),
                new("Металик", advertsFormatter.FormatMetallicExterior(advert.IsExteriorMetallic)),
                new("Област", advert.Region?.Name),
            };

            viewModel.InfoPairRows = infoPairRows.Where(pair => pair.Value != null);
            return View(viewModel);
        }
    }
}
