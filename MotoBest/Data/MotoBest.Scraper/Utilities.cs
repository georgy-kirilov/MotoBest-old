namespace MotoBest.Scraper
{
    using System;
    using System.Text;
    using System.Globalization;
    using System.Collections.Generic;

    public static class Utilities
    {
        public static readonly SortedDictionary<DateTime, string> EuroStandardsByDateTable = new()
        {
            { new DateTime(1992, 12, 31), "Евро 1" },
            { new DateTime(1997, 1, 1), "Евро 2" },
            { new DateTime(2001, 1, 1), "Евро 3" },
            { new DateTime(2006, 1, 1), "Евро 4" },
            { new DateTime(2011, 1, 1), "Евро 5" },
            { new DateTime(2015, 9, 1), "Евро 6" },
        };

        public static readonly CultureInfo BulgarianCultureInfo = new("bg-BG");

        public const string MonthNameDateFormat = "MMMM";

        public const string Whitespace = " ";

        public const string NewLine = "\n";

        public const decimal EuroToBgnExchangeRate = 1.95583M;

        public static string SanitizeText(string input, params string[] stringsToSanitize)
        {
            var builder = new StringBuilder(input);

            foreach (string stringToSanitize in stringsToSanitize)
            {
                builder.Replace(stringToSanitize, string.Empty);
            }

            return builder.ToString();
        }

        public static string EstimateEuroStandard(AdvertScrapeModel model)
        {
            model.IsEuroStandardExact = false;
            string currentEuroStandardType = null;

            foreach (var euroStandardDatePair in EuroStandardsByDateTable)
            {
                if (euroStandardDatePair.Key.CompareTo(model.ManufacturingDate) > 0)
                {
                    break;
                }

                currentEuroStandardType = euroStandardDatePair.Value;
            }

            return currentEuroStandardType;
        }
    }
}
