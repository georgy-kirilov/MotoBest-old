namespace MotoBest.Models
{
    using System.ComponentModel.DataAnnotations;

    using MotoBest.Models.Common;

    public class AdvertProvider : BaseNameableModel
    {
        [Required]
        public string AdvertUrlFormat { get; set; }
    }
}
