namespace MotoBest.Seeder.JsonModels
{
    using System.Collections.Generic;

    public class BrandJsonModel
    {
        public BrandJsonModel()
        {
            Models = new HashSet<ModelJsonModel>();
        }

        public string BrandName { get; set; }

        public ICollection<ModelJsonModel> Models { get; set; }
    }
}
