namespace MotoBest.Services.Contracts
{
    using System;

    public interface IAdvertsFormatter
    {
        string FormatEuroStandard(string euroStandard, bool isEuroStandardExact);

        string FormatManufacturingDate(DateTime? manufacturingDate);

        string FormatHorsePowers(int? horsePowers);

        string FormatMetallicExterior(bool isExteriorMetallic);

        string FormatKilometrage(long? kilometrage);

        string FormatDoorsCount(bool? hasFourDoors);

        string FormatDescription(string description, int symbolsCount = 200);

        string FormatPrice(decimal? price);
    }
}
