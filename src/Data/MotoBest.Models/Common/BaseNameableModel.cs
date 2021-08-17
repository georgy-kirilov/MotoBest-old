namespace MotoBest.Models.Common
{
    using System.ComponentModel.DataAnnotations;

    public abstract class BaseNameableModel : BaseOneToManyAdvertsModel
    {
        [Required]
        public string Name { get; set; }
    }
}
