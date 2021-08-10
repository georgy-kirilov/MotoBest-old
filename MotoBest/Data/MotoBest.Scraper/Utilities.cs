namespace MotoBest.Scraper
{
    using System.Globalization;

    public static class Utilities
    {
        public static readonly CultureInfo BulgarianCultureInfo = new("bg-BG");

        public const string MonthNameDateFormat = "MMMM";

        public const string Whitespace = " ";

        public const string NewLine = "\n";

        public const decimal EuroToBgnExchangeRate = 1.95583M;
    }
}
