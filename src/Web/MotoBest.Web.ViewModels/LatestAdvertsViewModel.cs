namespace MotoBest.Web.ViewModels
{
    using System.Collections.Generic;

    public class LatestAdvertsViewModel
    {
        public IEnumerable<AdvertViewModel> Adverts { get; set; }

        public int PageIndex { get; set; }
    }
}
