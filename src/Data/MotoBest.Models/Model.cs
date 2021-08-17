namespace MotoBest.Models
{
    using MotoBest.Models.Common;

    public class Model : BaseNameableModel
    {
        public int BrandId { get; set; }

        public virtual Brand Brand { get; set; }
    }
}