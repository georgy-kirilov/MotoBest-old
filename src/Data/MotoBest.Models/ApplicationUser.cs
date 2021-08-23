namespace MotoBest.Models
{
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Identity;

    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            SavedAdverts = new HashSet<Advert>();
        }

        public virtual ICollection<Advert> SavedAdverts { get; set; }
    }
}
