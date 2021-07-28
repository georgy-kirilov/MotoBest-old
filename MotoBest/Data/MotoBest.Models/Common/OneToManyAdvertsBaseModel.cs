namespace MotoBest.Models.Common
{
    using System.Collections.Generic;

    public abstract class OneToManyAdvertsBaseModel : BaseModel<int>
    {
        public OneToManyAdvertsBaseModel()
        {
            Adverts = new HashSet<Advert>();
        }

        public virtual ICollection<Advert> Adverts { get; set; }
    }
}
