namespace MotoBest.Scraper
{
    using System;
    using System.Threading.Tasks;

    using Common;

    public abstract class WebScraper : IWebScraper
    {
        protected WebScraper(string advertUrlFormat)
        {
            Validator.ThrowIfNullOrEmpty(advertUrlFormat, nameof(advertUrlFormat));
            AdvertUrlFormat = advertUrlFormat;
        }

        protected string GetAdvertUrl(string remoteId)
        {
            return string.Format(AdvertUrlFormat, remoteId);
        }

        public string AdvertUrlFormat { get; }

        public abstract Task<AdvertScrapeModel> ScrapeAdvertAsync(string remoteId);

        public abstract Task ScrapeAllAdvertsAsync(string brandName, string modelName, Action<AdvertScrapeModel> action);

        public abstract Task ScrapeLatestAdvertsAsync(Action<AdvertScrapeModel> action);
    }
}
