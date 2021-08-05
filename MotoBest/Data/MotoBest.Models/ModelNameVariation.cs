namespace MotoBest.Models
{
    using MotoBest.Models.Common;

    public class ModelNameVariation : NameableBaseModel
    {
        public int ModelId { get; set; }

        public virtual Model Model { get; set; }
    }
}
