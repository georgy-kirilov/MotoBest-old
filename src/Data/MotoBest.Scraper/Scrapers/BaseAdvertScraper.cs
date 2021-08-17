namespace MotoBest.Scraping.Scrapers
{
    using System;
    using System.Threading.Tasks;

    using AngleSharp;

    using MotoBest.Scraping.Common;
    using MotoBest.Common;

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

        public string AdvertUrlFormat { get; }

        public string AdvertProviderName { get; }

        public virtual Task<AdvertScrapeModel> ScrapeAdvertAsync(string remoteId)
        {
            return new Task<AdvertScrapeModel>(() => new AdvertScrapeModel
            {
                RemoteId = remoteId,
                AdvertProviderName = AdvertProviderName,
                AdvertUrlFormat = AdvertUrlFormat,
            });
        }

        public abstract Task ScrapeAllAdvertsAsync(Action<AdvertScrapeModel> action);

        public abstract Task ScrapeLatestAdvertsAsync(Action<AdvertScrapeModel> action);
    }
}
