namespace MotoBest.Data.Scraping.Common
{
    using System;

    using MotoBest.Seeding.Entities;

    using static MotoBest.Data.Scraping.Common.Utilities;
    using static MotoBest.Data.Scraping.Common.Utilities.Date;

    public static class PropertyParser
    {
        public static int? ParseHorsePowers(string input)
        {
            try
            {
                input = input?.ToLower();
                int horsePowers = int.Parse(SanitizeText(input, "к.с.", Characters.Whitespace));

                if (horsePowers == 0)
                {
                    return null;
                }

                return horsePowers;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static long? ParseKilometrage(string input)
        {
            try
            {
                input = input?.ToLower();
                long kilometrage = long.Parse(SanitizeText(input, "km", "км", Characters.Whitespace));

                if (kilometrage == 0)
                {
                    return null;
                }

                return kilometrage;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static decimal? ParsePrice(string input)
        {
            try
            {
                input = input?.ToLower();
                decimal exchangeRate = GetCurrencyExchangeRate(input);
                decimal price = decimal.Parse(SanitizeText(input, "лв.", "bgn", "eur", Characters.Whitespace));

                if (price == 0)
                {
                    return null;
                }

                return price * exchangeRate;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static DateTime? ParseManufacturingDate(string input)
        {
            try
            {
                input = SanitizeText(input, "г.").Trim();
                var args = input.Split(Characters.Whitespace, StringSplitOptions.RemoveEmptyEntries);

                int month = DateTime.ParseExact(args[0], MonthNameDateFormat, BulgarianCultureInfo).Month;
                int year = int.Parse(args[1]);

                return new DateTime(year, month, 1);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static void ParseColorNameAndExterior(string input, AdvertScrapeModel model)
        {
            model.ColorName = input?.Trim();
            model.IsExteriorMetallic = model.ColorName.Contains(Colors.Metallic, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
