using System.ComponentModel.DataAnnotations;

namespace MotoBest.Web.InputModels
{
    public class SearchAdvertsInputModel
    {
        [Display(Name = "Марка")]
        public int? BrandId { get; set; }

        [Display(Name = "Модел")]
        public int? ModelId { get; set; }

        [Display(Name = "Двигател")]
        public int? EngineId { get; set; }

        [Display(Name = "Скоростна кутия")]
        public int? TransmissionId { get; set; }

        [Display(Name = "Тип")]
        public int? BodyStyleId { get; set; }

        [Display(Name = "Състояние")]
        public int? ConditionId { get; set; }

        [Display(Name = "Евро стандарт")]
        public int? EuroStandardId { get; set; }

        [Display(Name = "Цвят")]
        public int? ColorId { get; set; }

        [Display(Name = "Регион")]
        public int? RegionId { get; set; }

        [Display(Name = "Населено място")]
        public int? TownId { get; set; }

        public int Page { get; set; }

        public decimal? MinPrice { get; set; }

        public decimal? MaxPrice { get; set; }

        public int? MinHorsePowers { get; set; }

        public int? MaxHorsePowers { get; set; }

        public long? MinKilometrage { get; set; }

        public long? MaxKilometrage { get; set; }
    }
}
