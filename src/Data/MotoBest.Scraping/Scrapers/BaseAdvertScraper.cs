namespace MotoBest.Scraping.Scrapers
{
    using System;
    using System.Threading.Tasks;

    using AngleSharp;

    using MotoBest.Scraping.Common;
    using MotoBest.Common;

    using static MotoBest.Scraping.Common.ScrapedDataNormalizer;

    public abstract class BaseAdvertScraper : IAdvertScraper
    {
        protected readonly IBrowsingContext browsingContext;

        protected BaseAdvertScraper(IBrowsingContext browsingContext, string advertUrlFormat, string advertProviderName)
        {
            Validator.ThrowIfNullOrEmpty(advertUrlFormat, nameof(advertUrlFormat));
            AdvertUrlFormat = advertUrlFormat;

            Validator.ThrowIfNullOrEmpty(advertProviderName, nameof(advertProviderName));
            AdvertProviderName = advertProviderName;

            Validator.ThrowIfNull(browsingContext, nameof(browsingContext));
            this.browsingContext = browsingContext;
        }

        protected string GetAdvertUrl(string remoteId)
        {
            return string.Format(AdvertUrlFormat, remoteId);
        }

        protected void NormalizeScrapeModel(AdvertScrapeModel model)
        {
            model.BodyStyleName = NormalizeBodyStyle(model.BodyStyleName);
            model.EngineType = NormalizeEngine(model.EngineType);
            model.Condition = NormalizeCondition(model.Condition);
            model.TransmissionType = NormalizeTransmission(model.TransmissionType);
            model.ColorName = NormalizeColor(model.ColorName);
            model.RegionName = NormalizeRegion(model.RegionName);
        }

        public string AdvertUrlFormat { get; }

        public string AdvertProviderName { get; }

        public virtual async Task<AdvertScrapeModel> ScrapeAdvertAsync(string remoteId)
        {
            return new AdvertScrapeModel
            {
                RemoteId = remoteId,
                AdvertProviderName = AdvertProviderName,
                AdvertUrlFormat = AdvertUrlFormat,
            };
        }

        public abstract Task ScrapeAllAdvertsAsync(Action<AdvertScrapeModel> action);

        public abstract Task ScrapeLatestAdvertsAsync(Action<AdvertScrapeModel> action);
    }
}
