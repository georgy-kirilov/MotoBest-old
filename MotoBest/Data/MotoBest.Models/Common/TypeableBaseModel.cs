namespace MotoBest.Models.Common
{
    using System.ComponentModel.DataAnnotations;

    public abstract class TypeableBaseModel : OneToManyAdvertsBaseModel
    {
        [Required]
        public string Type { get; set; }
    }
}
