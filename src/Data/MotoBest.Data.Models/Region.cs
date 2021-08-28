namespace MotoBest.Data.Models
{
    using System.Collections.Generic;

    using MotoBest.Data.Models.Common;

    public class Region : BaseNameableModel
    {
        public Region()
        {
            Towns = new HashSet<Town>();
        }

        public virtual ICollection<Town> Towns { get; set; }
    }
}
