﻿namespace MotoBest.Scraper
{
    using System;
    using System.Threading.Tasks;

    public interface IAdvertScraper
    {
        string AdvertUrlFormat { get; }

        Task<AdvertScrapeModel> ScrapeAdvertAsync(string remoteId);

        Task ScrapeLatestAdvertsAsync(Action<AdvertScrapeModel> action);

        Task ScrapeAllAdvertsAsync(Action<AdvertScrapeModel> action);
    }
}
