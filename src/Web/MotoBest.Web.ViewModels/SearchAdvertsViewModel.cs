namespace MotoBest.Web.ViewModels
{
    using System.Collections.Generic;

    using MotoBest.Services.DTOs;

    public class SearchAdvertsViewModel
    {
        public IEnumerable<BrandDto> Brands { get; set; }

        public IEnumerable<EngineDto> Engines { get; set; }

        public IEnumerable<TransmissionDto> Transmissions { get; set; }

        public IEnumerable<BodyStyleDto> BodyStyles { get; set; }

        public IEnumerable<ConditionDto> Conditions { get; set; }

        public IEnumerable<EuroStandardDto> EuroStandards { get; set; }

        public IEnumerable<ColorDto> Colors { get; set; }

        public IEnumerable<RegionDto> Regions { get; set; }
    }
}
