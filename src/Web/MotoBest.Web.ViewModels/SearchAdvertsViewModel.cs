namespace MotoBest.Web.ViewModels
{
    using System.Collections.Generic;

    public class SearchAdvertsViewModel
    {
        public IEnumerable<string> Brands { get; set; }

        public IEnumerable<string> Engines { get; set; }

        public IEnumerable<string> Transmissions { get; set; }

        public IEnumerable<string> BodyStyles { get; set; }

        public IEnumerable<string> Conditions { get; set; }

        public IEnumerable<string> EuroStandards { get; set; }

        public IEnumerable<string> Colors { get; set; }

        public IEnumerable<string> Regions { get; set; }
    }
}
