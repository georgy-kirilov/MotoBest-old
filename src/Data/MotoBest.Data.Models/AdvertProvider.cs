namespace MotoBest.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using MotoBest.Data.Models.Common;

    public class AdvertProvider : BaseNameableModel
    {
        [Required]
        public string AdvertUrlFormat { get; set; }
    }
}
