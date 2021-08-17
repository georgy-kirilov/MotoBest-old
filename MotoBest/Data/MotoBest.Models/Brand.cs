namespace MotoBest.Models
{
    using System.Collections.Generic;

    using MotoBest.Models.Common;

    public class Brand : BaseNameableModel
    {
        public Brand()
        {
            Models = new HashSet<Model>();
        }

        public virtual ICollection<Model> Models { get; set; }
    }
}
