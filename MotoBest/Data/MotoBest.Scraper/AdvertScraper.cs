namespace MotoBest.Scraper
{
    using Common;

    using System;
    using System.Threading.Tasks;

    public abstract class AdvertScraper : IAdvertScraper
    {
        protected AdvertScraper(string advertUrlFormat, string advertProviderName)
        {
            Validator.ThrowIfNullOrEmpty(advertUrlFormat, nameof(advertUrlFormat));
            Validator.ThrowIfNullOrEmpty(advertProviderName, nameof(advertProviderName));
            AdvertUrlFormat = advertUrlFormat;
            AdvertProviderName = advertProviderName;
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
            };
        }

        public abstract Task ScrapeAllAdvertsAsync(Action<AdvertScrapeModel> action);

        public abstract Task ScrapeLatestAdvertsAsync(Action<AdvertScrapeModel> action);
    }
}
