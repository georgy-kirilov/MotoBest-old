namespace MotoBest.Scraper
{
    using System.Text;
    using System.Globalization;

    public static class Utilities
    {
        public static readonly CultureInfo BulgarianCultureInfo = new("bg-BG");

        public const string MonthNameDateFormat = "MMMM";

        public const string Whitespace = " ";

        public const char NewLine = '\n';

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
    }
}
