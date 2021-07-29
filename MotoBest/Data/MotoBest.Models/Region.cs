namespace MotoBest.Models
{
    using System.Collections.Generic;

    using Common;

    public class Region : NameableBaseModel
    {
        public Region()
        {
            Towns = new HashSet<Town>();
        }

        public virtual ICollection<Town> Towns { get; set; }
    }
}
