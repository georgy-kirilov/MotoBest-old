namespace MotoBest.Models
{
    using System.Collections.Generic;

    using MotoBest.Models.Common;

    public class Region : BaseNameableModel
    {
        public Region()
        {
            Towns = new HashSet<Town>();
        }

        public virtual ICollection<Town> Towns { get; set; }
    }
}
