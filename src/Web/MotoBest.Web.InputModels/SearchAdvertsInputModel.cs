namespace MotoBest.Web.InputModels
{
    public class SearchAdvertsInputModel
    {
        public string Brand { get; set; }

        public string Model { get; set; }

        public string Engine { get; set; }

        public string Transmission { get; set; }

        public string BodyStyle { get; set; }

        public string Condition { get; set; }

        public string EuroStandard { get; set; }

        public string Color { get; set; }

        public string Region { get; set; }

        public string Town { get; set; }

        public decimal? MinPrice { get; set; }

        public decimal? MaxPrice { get; set; }
    }
}
