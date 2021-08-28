namespace MotoBest.Data.Models
{
    using MotoBest.Data.Models.Common;

    public class Town : BaseNameableModel
    {
        public int RegionId { get; set; }

        public virtual Region Region { get; set; }
    }
}
