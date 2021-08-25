namespace MotoBest.Services
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Globalization;

    using MotoBest.Common;

    using static MotoBest.Scraping.Common.Utilities.Date;
    using static MotoBest.Scraping.Common.Utilities.Characters;

    public class AdvertsFormatter : IAdvertsFormatter
    {
        public string FormatDescription(string description)
        {
            int symbolsCount = 300;

            if (description == null)
            {
                return null;
            }

            if (description.Length > symbolsCount)
            {
                return $"{description.Substring(0, symbolsCount)}...";
            }

            return description;
        }

        public string FormatDoorsCount(bool? hasFourDoors)
        {
            if (hasFourDoors == null)
            {
                return null;
            }

            return hasFourDoors.Value ? "4/5" : "2/3";
        }

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

            return $"{horsePowers} к.с.";
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
                    builder.Append(Whitespace);
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
            return manufacturingDate?.ToString(FullMonthNameAndYearFormat, BulgarianCultureInfo).Capitalize();
        }

        public string FormatMetallicExterior(bool isExteriorMetallic)
        {
            return isExteriorMetallic ? "Да" : "Не";
        }

        public string FormatPrice(decimal? price)
        {
            var format = new NumberFormatInfo { NumberGroupSeparator = Whitespace };
            return price?.ToString("N", format).Replace(".", Comma) + Whitespace + "лв." ?? "По Договаряне";
        }
    }
}
