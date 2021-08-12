namespace MotoBest.Scraper
{
    using AngleSharp;
    using Common;

    using System;
    using System.Threading.Tasks;

    public abstract class AdvertScraper : IAdvertScraper
    {
        protected readonly IBrowsingContext browsingContext;

        protected AdvertScraper(IBrowsingContext browsingContext, string advertUrlFormat, string advertProviderName)
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
