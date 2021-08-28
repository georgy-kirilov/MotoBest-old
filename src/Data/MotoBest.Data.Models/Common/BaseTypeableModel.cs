namespace MotoBest.Data.Models.Common
{
    using System.ComponentModel.DataAnnotations;

    public abstract class BaseTypeableModel : BaseOneToManyAdvertsModel
    {
        [Required]
        public string Type { get; set; }
    }
}
