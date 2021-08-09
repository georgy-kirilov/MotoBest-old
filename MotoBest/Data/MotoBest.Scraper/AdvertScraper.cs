namespace MotoBest.Scraper
{
    using System;
    using System.Threading.Tasks;

    using Common;

    public abstract class AdvertScraper : IWebScraper
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

        public abstract Task<AdvertScrapeModel> ScrapeAdvertAsync(string remoteId);

        public abstract Task ScrapeAllAdvertsAsync(string brandName, string modelName, Action<AdvertScrapeModel> action);

        public abstract Task ScrapeLatestAdvertsAsync(Action<AdvertScrapeModel> action);
    }
}
