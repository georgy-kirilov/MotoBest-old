namespace MotoBest.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using MotoBest.Models;
    using MotoBest.Scraping.Common;
    using MotoBest.Services;
    using MotoBest.Web.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using static MotoBest.Scraping.Common.Utilities.Date;

    public class AdvertsController : Controller
    {
        private readonly IAdvertsService advertsService;

        public AdvertsController(IAdvertsService advertsService)
        {
            this.advertsService = advertsService;
        }

        public string Capitalize(string input)
        {
            return $"{input[0].ToString().ToUpper()}{input[1..]}";
        }

        public string Commas(long? kilometrage)
        {
            if (kilometrage == null)
            {
                return null;
            }

            int counter = 0;
            string kilometrageAsString = kilometrage.ToString();
            var builder = new StringBuilder();

            for (int i = kilometrageAsString.Length - 1; i >= 0; i--)
            {
                if (counter == 3)
                {
                    builder.Append(',');
                    counter = -1;
                    i++;
                }
                else
                {
                    builder.Append(kilometrageAsString[i]);
                }

                counter++;
            }

            string value = new string(builder.ToString().ToCharArray().Reverse().ToArray());

            return $"{value} км";
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
                Brand = advert.Brand.Name,
                Model = advert.Model.Name,
                Price = advert.Price,
                HorsePowers = advert.HorsePowers,
                Kilometrage = advert.Kilometrage,
                Transmission = advert.Transmission.Type,
                BodyStyle = advert.BodyStyle.Name,
                Engine = advert.Engine.Type,
                Color = advert.Color.Name,
                EuroStandard = advert.EuroStandard.Type,
                IsEuroStandardExact = advert.IsEuroStandardExact,
                Region = advert.Region.Name,
                ManufacturingDate = advert.ManufacturingDate?.ToString("MMMM yyyy"),
                ImageUrls = advert.Images.Select(i => i.Url),
            };

            var keyValuePairsInfoRows = new Dictionary<string, string>
            {
                { "Дата на производство", Capitalize(advert.ManufacturingDate?.ToString(FullMonthNameAndYearFormat, BulgarianCultureInfo)) },
                { "Марка", advert.Brand.Name },
                { "Модел", advert.Model.Name },
                { "Скоростна кутия", Capitalize(advert.Transmission.Type) },
                { "Двигател", Capitalize(advert.Engine.Type) },
                { "Тип", Capitalize(advert.BodyStyle.Name) },
                { "Евро стандарт", Capitalize(advert.EuroStandard.Type) },
                { "Конски сили",  advert.HorsePowers.ToString() },
                { "Пробег", Commas(advert.Kilometrage) },
                { "Цвят", Capitalize(advert.Color.Name) },
            };

            viewModel.KeyValuePairInfoRows = keyValuePairsInfoRows;

            return View(viewModel);
        }
    }
}
