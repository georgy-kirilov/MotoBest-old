namespace MotoBest.Models
{
    using System.Collections.Generic;

    using Models.Common;

    public class Brand : NameableBaseModel
    {
        public Brand()
        {
            Models = new HashSet<Model>();
        }

        public virtual ICollection<Model> Models { get; set; }
    }
}
