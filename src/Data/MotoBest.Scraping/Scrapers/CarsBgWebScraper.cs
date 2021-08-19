namespace MotoBest.Scraping.Scrapers
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    using AngleSharp;

    using MotoBest.Scraping.Common;

    using static MotoBest.Scraping.Common.Utilities;

    public class CarsBgWebScraper : BaseWebScraper
    {
        public const string CarsBgAdvertProviderName = "cars.bg";
        public const string CarsBgAdvertUrlFormat = "https://www.cars.bg/offer/{0}";

        public CarsBgWebScraper(IBrowsingContext browsingContext) 
            : base(browsingContext, CarsBgAdvertUrlFormat, CarsBgAdvertProviderName)
        {
        }

        public override async Task<AdvertScrapeModel> ScrapeAdvertAsync(string remoteId)
        {
            string url = GetAdvertUrl(remoteId);
            string html = await GetHtmlAsync(url, UserAgentHeader);

            await File.WriteAllTextAsync("C:/Users/georg/OneDrive/Desktop/cars-bg-advert.html", html);

            var document = await browsingContext.OpenAsync(res => res.Content(html));
            AdvertScrapeModel model = await base.ScrapeAdvertAsync(remoteId);

            return model;
        }

        public override async Task ScrapeAllAdvertsAsync(Action<AdvertScrapeModel> action)
        {
            throw new NotImplementedException();
        }

        public override async Task ScrapeLatestAdvertsAsync(Action<AdvertScrapeModel> action)
        {
            throw new NotImplementedException();
        }
    }
}
