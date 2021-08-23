namespace MotoBest.Services
{
    using System;

    public interface IAdvertsFormatter
    {
        string FormatEuroStandard(string euroStandard, bool isEuroStandardExact);

        string FormatManufacturingDate(DateTime? manufacturingDate);

        string FormatHorsePowers(int? horsePowers);

        string FormatMetallicExterior(bool isExteriorMetallic);

        string FormatKilometrage(long? kilometrage);
    }
}
