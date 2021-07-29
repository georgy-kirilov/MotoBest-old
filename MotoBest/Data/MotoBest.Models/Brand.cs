namespace MotoBest.Models
{
    using System.Collections.Generic;

    using Common;

    public class Brand : NameableBaseModel
    {
        public Brand()
        {
            Models = new HashSet<Model>();
        }

        public virtual ICollection<Model> Models { get; set; }
    }
}
