namespace MotoBest.Data.Models
{
    using System.Collections.Generic;

    using MotoBest.Data.Models.Common;

    public class Brand : BaseNameableModel
    {
        public Brand()
        {
            Models = new HashSet<Model>();
        }

        public virtual ICollection<Model> Models { get; set; }
    }
}
