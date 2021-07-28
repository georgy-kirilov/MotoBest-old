namespace MotoBest.Models.Common
{
    using System.ComponentModel.DataAnnotations;

    public abstract class NameableBaseModel : BaseModel<int>
    {
        [Required]
        public string Name { get; set; }
    }
}
