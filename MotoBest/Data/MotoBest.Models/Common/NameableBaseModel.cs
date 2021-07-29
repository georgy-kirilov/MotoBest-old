namespace MotoBest.Models.Common
{
    using System.ComponentModel.DataAnnotations;

    public abstract class NameableBaseModel : OneToManyAdvertsBaseModel
    {
        [Required]
        public string Name { get; set; }
    }
}
