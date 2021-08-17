namespace MotoBest.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using MotoBest.Models.Common;

    public class Image : BaseModel<int>
    {
        [Required]
        public string Url { get; set; }

        public Guid AdvertId { get; set; }

        public virtual Advert Advert { get; set; }
    }
}
