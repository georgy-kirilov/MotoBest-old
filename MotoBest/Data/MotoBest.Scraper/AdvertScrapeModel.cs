namespace MotoBest.Scraper
{
    using System;
    using System.Collections.Generic;

    public class AdvertScrapeModel
    {
        public AdvertScrapeModel()
        {
            ImageUrls = new HashSet<string>();
        }

        public string RemoteId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public decimal? Price { get; set; }

        public int HorsePowers { get; set; }

        public int Kilometrage { get; set; }

        public int Views { get; set; }

        public DateTime ManufacturingDate { get; set; }

        public DateTime LastModifiedOn { get; set; }

        public string BrandName { get; set; }

        public string ModelName { get; set; }

        public string TransmissionType { get; set; }

        public string EngineType { get; set; }

        public string BodyStyleName { get; set; }

        public string EuroStandardType { get; set; }

        public string ColorName { get; set; }

        public string RegionName { get; set; }

        public string TownName { get; set; }

        public HashSet<string> ImageUrls { get; set; }
    }
}
