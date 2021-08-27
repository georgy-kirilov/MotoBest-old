﻿namespace MotoBest.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using MotoBest.Data;
    using MotoBest.Models;
    using MotoBest.Common;
    using MotoBest.Web.ViewModels;
    using MotoBest.Scraping.Common;
    using MotoBest.Web.InputModels;
    using MotoBest.Models.Common;

    public class AdvertsService : IAdvertsService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IModelFactory modelFactory;
        private readonly IAdvertsFormatter advertsFormatter;

        public AdvertsService(ApplicationDbContext dbContext, IModelFactory modelFactory, IAdvertsFormatter advertsFormatter)
        {
            this.dbContext = dbContext;
            this.modelFactory = modelFactory;
            this.advertsFormatter = advertsFormatter;
        }

        public async Task AddOrUpdateAdvertAsync(AdvertScrapeModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            AdvertProvider advertProvider = modelFactory.GetOrCreateAdvertProvider(model);

            Advert advert = advertProvider.Adverts.FirstOrDefault(a => a.RemoteId == model.RemoteId);
            bool isAdvertNew = false;

            if (advert == null)
            {
                advert = new Advert { RemoteId = model.RemoteId };
                isAdvertNew = true;
            }

            advert.AdvertProvider = advertProvider;

            MapNavigationalProperties(advert, model);
            MapSimpleProperties(advert, model);

            foreach (string imageUrl in model.ImageUrls)
            {
                Image image = modelFactory.GetOrCreateImage(imageUrl, advert);
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
                Brands = SelectNameableModels(dbContext.Brands),
                Engines = SelectTypeableModels(dbContext.Engines),
                Transmissions = SelectTypeableModels(dbContext.Transmissions),
                BodyStyles = SelectNameableModels(dbContext.BodyStyles),
                Conditions = SelectTypeableModels(dbContext.Conditions),
                Colors = SelectNameableModels(dbContext.Colors),
                EuroStandards = SelectTypeableModels(dbContext.EuroStandards),
                Regions = SelectNameableModels(dbContext.Regions),
            };

            return viewModel;
        }

        public Advert GetAdvertById(string id)
        {
            return dbContext.Adverts.FirstOrDefault(a => a.Id.ToString() == id);
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

        public IEnumerable<AdvertViewModel> SearchForAdverts(SearchAdvertsInputModel input)
        {
            var adverts = dbContext.Adverts.Where(advert =>
                                    (input.Brand == null || advert.Brand.Name == input.Brand) &&
                                    (input.Model == null || advert.Model.Name == input.Model) &&
                                    (input.Engine == null || advert.Engine.Type == input.Engine) &&
                                    (input.BodyStyle == null || advert.BodyStyle.Name == input.BodyStyle) &&
                                    (input.Color == null || advert.Color.Name == input.Color) &&
                                    (input.Transmission == null || advert.Transmission.Type == input.Transmission) &&
                                    (input.Condition == null || advert.Condition.Type == input.Condition) &&
                                    (input.EuroStandard == null || advert.EuroStandard.Type == input.EuroStandard) &&
                                    (input.Region == null || advert.Region.Name == input.Region))
                                    .Take(10)
                                    .Select(MapViewModelFrom)
                                    .ToList();
            return adverts;
        }

        private void MapNavigationalProperties(Advert advert, AdvertScrapeModel model)
        {
            advert.Brand = modelFactory.GetOrCreateBrand(model.BrandName);
            advert.Model = modelFactory.GetOrCreateModel(model.ModelName, advert.Brand);
            advert.Color = modelFactory.GetOrCreateColor(model.ColorName);
            advert.Engine = modelFactory.GetOrCreateEngine(model.EngineType);
            advert.Transmission = modelFactory.GetOrCreateTransmission(model.TransmissionType);
            advert.BodyStyle = modelFactory.GetOrCreateBodyStyle(model.BodyStyleName);
            advert.Region = modelFactory.GetOrCreateRegion(model.RegionName);
            advert.Town = modelFactory.GetOrCreateTown(model.TownName, advert.Region);
            advert.EuroStandard = modelFactory.GetOrCreateEuroStandard(model.EuroStandardType);
            advert.Condition = modelFactory.GetOrCreateCondition(model.Condition);
        }

        private void MapSimpleProperties(Advert advert, AdvertScrapeModel model)
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

        private static IEnumerable<string> SelectNameableModels<T>(IQueryable<T> queryable) where T : BaseNameableModel
        {
            return queryable.OrderBy(model => model.Name).Select(model => model.Name);
        }

        private static IEnumerable<string> SelectTypeableModels<T>(IQueryable<T> queryable) where T : BaseTypeableModel
        {
            return queryable.OrderBy(model => model.Type).Select(model => model.Type);
        }
    }
}
