namespace MotoBest.Services
{
    using System;
    using System.Linq;
    using System.Text;

    using MotoBest.Common;

    using static MotoBest.Scraping.Common.Utilities.Date;
    using static MotoBest.Scraping.Common.Utilities.Characters;

    public class AdvertsFormatter : IAdvertsFormatter
    {
        public string FormatEuroStandard(string euroStandard, bool isEuroStandardExact)
        {
            if (euroStandard == null)
            {
                return null;
            }

            string extension = isEuroStandardExact ? string.Empty : "*";
            return $"{euroStandard.Capitalize()}{extension}";
        }

        public string FormatHorsePowers(int? horsePowers)
        {
            if (horsePowers == null)
            {
                return null;
            }

            return $"{horsePowers} конски сили";
        }

        public string FormatKilometrage(long? kilometrage)
        {
            if (kilometrage == null)
            {
                return null;
            }

            int counter = 1;
            var kilometrageAsString = kilometrage.ToString();
            var builder = new StringBuilder();

            for (int i = kilometrageAsString.Length - 1; i >= 0; i--)
            {
                if (counter > 3)
                {
                    builder.Append(Comma);
                    counter = 0;
                    i++;
                }
                else
                {
                    builder.Append(kilometrageAsString[i]);
                }

                counter++;
            }

            string value = new(builder.ToString().ToCharArray().Reverse().ToArray());
            return $"{value} км";
        }

        public string FormatManufacturingDate(DateTime? manufacturingDate)
        {
            if (manufacturingDate == null)
            {
                return null;
            }

            return manufacturingDate?.ToString(FullMonthNameAndYearFormat, BulgarianCultureInfo).Capitalize();
        }

        public string FormatMetallicExterior(bool isExteriorMetallic)
        {
            return isExteriorMetallic ? "Да" : "Не";
        }
    }
}
