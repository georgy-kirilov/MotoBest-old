namespace MotoBest.Web.ViewModels
{
    using System.Collections.Generic;

    public class SearchResultsViewModel
    {
        public IEnumerable<AdvertViewModel> Adverts { get; set; }

        public int PageIndex { get; set; }
    }
}
