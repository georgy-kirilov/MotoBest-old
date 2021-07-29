namespace MotoBest.Models
{
    using Common;

    public class Town : NameableBaseModel
    {
        public int RegionId { get; set; }

        public virtual Region Region { get; set; }
    }
}
