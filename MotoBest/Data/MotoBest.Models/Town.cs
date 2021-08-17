namespace MotoBest.Models
{
    using MotoBest.Models.Common;

    public class Town : BaseNameableModel
    {
        public int RegionId { get; set; }

        public virtual Region Region { get; set; }
    }
}
