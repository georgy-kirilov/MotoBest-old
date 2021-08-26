namespace MotoBest.Web.ViewModels
{
    using System;
    using System.Collections.Generic;

    public class AdvertViewModel
    {
        public AdvertViewModel()
        {
            ImageUrls = new HashSet<string>();
            InfoPairRows = new List<KeyValuePair<string, string>>();
        }

        public string Id { get; set; }

        public string Title { get; set; }

        public string LongDescription { get; set; }

        public string ShortDescription { get; set; }

        public string Price { get; set; }

        public string OriginalAdvertUrl { get; set; }

        public string AdvertProviderName { get; set; }

        public DateTime? LastModifiedOn { get; set; }

        public ICollection<string> ImageUrls { get; set; }

        public IEnumerable<KeyValuePair<string, string>> InfoPairRows { get; set; }
    }
}
