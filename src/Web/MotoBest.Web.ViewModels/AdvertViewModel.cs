using System.Collections.Generic;

namespace MotoBest.Web.ViewModels
{
    public class AdvertViewModel
    {
        public IEnumerable<KeyValuePair<string, string>> KeyValuePairInfoRows { get; set; }

        public string Title { get; set; }

        public string Brand { get; set; }

        public string Model { get; set; }

        public string Description { get; set; }

        public decimal? Price { get; set; }

        public int? HorsePowers { get; set; }

        public long? Kilometrage { get; set; }

        public string BodyStyle { get; set; }

        public string ManufacturingDate { get; set; }

        public string Transmission { get; set; }

        public string Engine { get; set; }

        public string Color { get; set; }

        public string EuroStandard { get; set; }

        public bool IsEuroStandardExact { get; set; }

        public string Region { get; set; }

        public IEnumerable<string> ImageUrls { get; set; }
    }
}
