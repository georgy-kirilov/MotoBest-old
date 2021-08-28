namespace MotoBest.Data.Models
{
    using MotoBest.Data.Models.Common;

    public class Model : BaseNameableModel
    {
        public int BrandId { get; set; }

        public virtual Brand Brand { get; set; }
    }
}