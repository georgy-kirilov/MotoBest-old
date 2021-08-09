namespace MotoBest.Models
{
    using Common;

    public class Model : NameableBaseModel
    {
        public int BrandId { get; set; }

        public virtual Brand Brand { get; set; }
    }
}