namespace MotoBest.Web.ViewModels
{
    using System.Collections.Generic;

    public class AdvertViewModel
    {
        public AdvertViewModel()
        {
            ImageUrls = new HashSet<string>();
            InfoPairRows = new List<KeyValuePair<string, string>>();
        }

        public string Title { get; set; }

        public string Description { get; set; }

        public decimal? Price { get; set; }

        public IEnumerable<string> ImageUrls { get; set; }

        public IEnumerable<KeyValuePair<string, string>> InfoPairRows { get; set; }
    }
}
