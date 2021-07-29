namespace MotoBest.Models
{
    using Common;
    using System.ComponentModel.DataAnnotations;

    public class AdvertProvider : NameableBaseModel
    {
        [Required]
        public string AdvertUrlFormat { get; set; }
    }
}
