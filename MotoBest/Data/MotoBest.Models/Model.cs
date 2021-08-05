namespace MotoBest.Models
{
    using Common;
    using System.Collections.Generic;

    public class Model : NameableBaseModel
    {
        public Model()
        {
            NameVariations = new HashSet<ModelNameVariation>();
        }

        public int BrandId { get; set; }

        public virtual Brand Brand { get; set; }

        public virtual ICollection<ModelNameVariation> NameVariations { get; set; }
    }
}