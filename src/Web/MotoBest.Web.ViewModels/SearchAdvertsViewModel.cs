namespace MotoBest.Web.ViewModels
{
    using System.Collections.Generic;

    public class SearchAdvertsViewModel
    {
        public IEnumerable<KeyValuePair<string, IEnumerable<string>>> KeyValuePairs { get; set; }
    }
}
