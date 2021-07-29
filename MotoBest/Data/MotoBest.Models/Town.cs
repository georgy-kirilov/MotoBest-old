namespace MotoBest.Models
{
    using Common;

    public class Town : NameableBaseModel
    {
        public int RegionId { get; set; }

        public Region Region { get; set; }
    }
}
