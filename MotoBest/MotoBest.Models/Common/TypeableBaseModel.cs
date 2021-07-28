namespace MotoBest.Models.Common
{
    using System.ComponentModel.DataAnnotations;

    public abstract class TypeableBaseModel : BaseModel<int>
    {
        [Required]
        public string Type { get; set; }
    }
}
