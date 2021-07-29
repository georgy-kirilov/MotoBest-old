namespace MotoBest.Models
{
    using Common;

    public class Model : NameableBaseModel
    {
        public int BrandId { get; set; }

        public Brand Brand { get; set; }
    }
}