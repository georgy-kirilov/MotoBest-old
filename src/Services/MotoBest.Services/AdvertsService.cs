namespace MotoBest.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using MotoBest.Data;
    using MotoBest.Common;
    using MotoBest.Data.Models;
    using MotoBest.Web.ViewModels;
    using MotoBest.Web.InputModels;
    using MotoBest.Data.Scraping.Common;
    using MotoBest.Services.Contracts;

    public class AdvertsService : IAdvertsService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IAdvertsFormatter advertsFormatter;
        private readonly IBrandsService brandsService;
        private readonly IModelsService modelsService;
        private readonly IColorsService colorsService;
        private readonly IEnginesService enginesService;
        private readonly ITransmissionsService transmissionsService;
        private readonly IBodyStylesService bodyStylesService;
        private readonly IEuroStandardsService euroStandardsService;
        private readonly IConditionsService conditionsService;
        private readonly ITownsService townsService;
        private readonly IRegionsService regionsService;
        private readonly IImagesService imagesService;
        private readonly IAdvertProvidersService advertProvidersService;

        public AdvertsService(
            ApplicationDbContext dbContext, 
            IAdvertsFormatter advertsFormatter, 
            IBrandsService brandsService, 
            IModelsService modelsService, 
            IColorsService colorsService, 
            IEnginesService enginesService, 
            ITransmissionsService transmissionsService, 
            IBodyStylesService bodyStylesService, 
            IEuroStandardsService euroStandardsService, 
            IConditionsService conditionsService, 
            ITownsService townsService, 
            IRegionsService regionsService, 
            IImagesService imagesService, 
            IAdvertProvidersService advertProvidersService)
        {
            this.dbContext = dbContext;
            this.advertsFormatter = advertsFormatter;
            this.brandsService = brandsService;
            this.modelsService = modelsService;
            this.colorsService = colorsService;
            this.enginesService = enginesService;
            this.transmissionsService = transmissionsService;
            this.bodyStylesService = bodyStylesService;
            this.euroStandardsService = euroStandardsService;
            this.conditionsService = conditionsService;
            this.townsService = townsService;
            this.regionsService = regionsService;
            this.imagesService = imagesService;
            this.advertProvidersService = advertProvidersService;
        }

        public async Task AddOrUpdateAdvertAsync(AdvertScrapeModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var provider = advertProvidersService.GetOrCreate(model);

            Advert advert = provider.Adverts.FirstOrDefault(a => a.RemoteId == model.RemoteId);
            bool isAdvertNew = false;

            if (advert == null)
            {
                advert = new Advert { RemoteId = model.RemoteId };
                isAdvertNew = true;
            }

            advert.AdvertProvider = provider;

            MapComplexNavigationalProperties(advert, model);
            MapSimpleSingleProperties(advert, model);

            foreach (string imageUrl in model.ImageUrls)
            {
                Image image = imagesService.GetOrCreate(imageUrl, advert);
                advert.Images.Add(image);
            }

            if (isAdvertNew)
            {
                await dbContext.Adverts.AddAsync(advert);
            }

            await dbContext.SaveChangesAsync();
        }

        public SearchAdvertsViewModel CreateSearchAdvertsViewModel()
        {
            var viewModel = new SearchAdvertsViewModel
            {
                Brands = brandsService.GetAll(),
                Engines = enginesService.GetAll(),
                Transmissions = transmissionsService.GetAll(),
                BodyStyles = bodyStylesService.GetAll(),
                Conditions = conditionsService.GetAll(),
                Colors = colorsService.GetAll(),
                EuroStandards = euroStandardsService.GetAll(),
                Regions = regionsService.GetAll(),
            };

            return viewModel;
        }

        public Advert GetAdvertById(string id)
        {
            return dbContext.Adverts.FirstOrDefault(advert => advert.Id.ToString() == id);
        }

        public IEnumerable<AdvertViewModel> GetLatestAdverts(int pageIndex)
        {
            int advertsPerPageCount = 10;

            if (pageIndex <= 0)
            {
                pageIndex = 1;
            }

            int advertsToSkipCount = advertsPerPageCount * pageIndex;

            return dbContext.Adverts
                            .OrderByDescending(a => a.LastModifiedOn)
                            .Skip(advertsToSkipCount)
                            .Take(advertsPerPageCount)
                            .ToList()
                            .Select(MapViewModelFrom);
        }

        public int GetAllAdvertsCount()
        {
            return dbContext.Adverts.Count();
        }

        public AdvertViewModel MapViewModelFrom(Advert advert)
        {
            if (advert == null)
            {
                return new AdvertViewModel();
            }

            var viewModel = new AdvertViewModel
            {
                Id = advert.Id.ToString(),
                Title = advert.Title,
                LongDescription = advert.Description,
                ShortDescription = advertsFormatter.FormatDescription(advert.Description),
                AdvertProviderName = advert.AdvertProvider?.Name,
                OriginalAdvertUrl = string.Format(advert.AdvertProvider.AdvertUrlFormat, advert.RemoteId),
                Price = advertsFormatter.FormatPrice(advert.Price),
                LastModifiedOn = advert.LastModifiedOn,
                ImageUrls = advert.Images.Select(i => i.Url).ToList(),
            };

            var infoPairRows = new List<KeyValuePair<string, string>>
            {
                new("Марка", advert.Brand?.Name),
                new("Модел", advert.Model?.Name),
                new("Дата на производство", advertsFormatter.FormatManufacturingDate(advert.ManufacturingDate)),
                new("Състояние", advert.Condition?.Type.Capitalize()),
                new("Скоростна кутия", advert.Transmission?.Type.Capitalize()),
                new("Двигател", advert.Engine?.Type.Capitalize()),
                new("Тип", advert.BodyStyle?.Name.Capitalize()),
                new("Евро стандарт", advertsFormatter.FormatEuroStandard(advert.EuroStandard?.Type, advert.IsEuroStandardExact)),
                new("Врати", advertsFormatter.FormatDoorsCount(advert.HasFourDoors)),
                new("Мощност",  advertsFormatter.FormatHorsePowers(advert.HorsePowers)),
                new("Пробег", advertsFormatter.FormatKilometrage(advert.Kilometrage)),
                new("Цвят", advert.Color?.Name.Capitalize()),
                new("Област", advert.Region?.Name),
                new("Населено място", advert.Town?.Name),
            };

            if (viewModel.ImageUrls.Count == 0)
            {
                viewModel.ImageUrls.Add("/img/default-advert-img.jpg");
            }

            viewModel.InfoPairRows = infoPairRows.Where(pair => pair.Value != null);
            return viewModel;
        }

        public IEnumerable<AdvertViewModel> SearchAdverts(SearchAdvertsInputModel input)
        {
            int advertsPerPage = 10, advertsToSkip = advertsPerPage * input.Page;

            var inputProperties = input.GetType().GetProperties();
            var modelProperties = typeof(Advert).GetProperties();

            return dbContext.Adverts.Where(advert =>
                                        (input.BrandId == null || advert.BrandId == input.BrandId) &&
                                        (input.ModelId == null || advert.ModelId == input.ModelId) &&
                                        (input.EngineId == null || advert.EngineId == input.EngineId) &&
                                        (input.BodyStyleId == null || advert.BodyStyleId == input.BodyStyleId) &&
                                        (input.ColorId == null || advert.ColorId == input.ColorId) &&
                                        (input.TransmissionId == null || advert.TransmissionId == input.TransmissionId) &&
                                        (input.ConditionId == null || advert.ConditionId == input.ConditionId) &&
                                        (input.EuroStandardId == null || advert.EuroStandardId == input.EuroStandardId) &&
                                        (input.RegionId == null || advert.RegionId == input.RegionId) &&
                                        (input.TownId == null || advert.TownId == input.TownId))
                                    .Skip(advertsToSkip)
                                    .Take(advertsPerPage)
                                    .Select(MapViewModelFrom)
                                    .ToList();
        }

        private void MapComplexNavigationalProperties(Advert advert, AdvertScrapeModel model)
        {
            advert.Brand = brandsService.GetOrCreate(model.BrandName);
            advert.Model = modelsService.GetOrCreate(advert.Brand, model.ModelName);

            advert.Color = colorsService.GetOrCreate(model.ColorName);
            advert.Engine = enginesService.GetOrCreate(model.EngineType);

            advert.Transmission = transmissionsService.GetOrCreate(model.TransmissionType);
            advert.BodyStyle = bodyStylesService.GetOrCreate(model.BodyStyleName);

            advert.EuroStandard = euroStandardsService.GetOrCreate(model.EuroStandardType);
            advert.Condition = conditionsService.GetOrCreate(model.Condition);

            advert.Region = regionsService.GetOrCreate(model.RegionName);
            advert.Town = townsService.GetOrCreate(advert.Region, model.TownName);
        }

        private static void MapSimpleSingleProperties(Advert advert, AdvertScrapeModel model)
        {
            advert.Views = model.Views;
            advert.Kilometrage = model.Kilometrage;
            advert.HorsePowers = model.HorsePowers;
            advert.Title = model.Title;
            advert.Description = model.Description;
            advert.Price = model.Price;
            advert.ManufacturingDate = model.ManufacturingDate;
            advert.LastModifiedOn = model.LastModifiedOn;
            advert.IsNewImport = model.IsNewImport;
            advert.HasFourDoors = model.HasFourDoors;
            advert.IsEuroStandardExact = model.IsEuroStandardExact;
            advert.IsExteriorMetallic = model.IsExteriorMetallic;
        }
    }
}
