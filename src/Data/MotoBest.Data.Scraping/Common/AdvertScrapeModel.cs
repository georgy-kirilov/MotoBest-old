namespace MotoBest.Data.Scraping.Common
{
    using System;
    using System.Collections.Generic;

    using MotoBest.Data.Seeding.Entities;

    public class AdvertScrapeModel
    {
        public AdvertScrapeModel()
        {
            ImageUrls = new HashSet<string>();
            IsEuroStandardExact = true;
            Condition = Conditions.Used;
        }

        public string AdvertUrlFormat { get; set; }

        public string AdvertProviderName { get; set; }

        public string RemoteId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public decimal? Price { get; set; }

        public int? HorsePowers { get; set; }

        public long? Kilometrage { get; set; }

        public int? Views { get; set; }

        public DateTime? ManufacturingDate { get; set; }

        public DateTime? LastModifiedOn { get; set; }

        public string BrandName { get; set; }

        public string ModelName { get; set; }

        public string TransmissionType { get; set; }

        public string EngineType { get; set; }

        public string BodyStyleName { get; set; }

        public string EuroStandardType { get; set; }

        public bool IsEuroStandardExact { get; set; }

        public bool IsExteriorMetallic { get; set; }

        public string ColorName { get; set; }

        public string Condition { get; set; }

        public string RegionName { get; set; }

        public string TownName { get; set; }

        public bool? HasFourDoors { get; set; }

        public bool IsNewImport { get; set; }

        public HashSet<string> ImageUrls { get; set; }
    }
}
