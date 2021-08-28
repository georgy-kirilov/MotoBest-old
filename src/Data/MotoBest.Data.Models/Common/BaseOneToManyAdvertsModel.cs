namespace MotoBest.Data.Models.Common
{
    using System.Collections.Generic;

    public abstract class BaseOneToManyAdvertsModel : BaseModel<int>
    {
        public BaseOneToManyAdvertsModel()
        {
            Adverts = new HashSet<Advert>();
        }

        public virtual ICollection<Advert> Adverts { get; set; }
    }
}
