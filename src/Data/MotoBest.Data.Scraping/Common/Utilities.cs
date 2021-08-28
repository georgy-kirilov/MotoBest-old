namespace MotoBest.Data.Scraping.Common
{
    using System.Text;
    using System.Net.Http;
    using System.Globalization;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    public static class Utilities
    {
        public static class Characters
        {
            public const string Whitespace = " ";
            public const string Colon = ":";
            public const char NewLine = '\n';
            public const string Comma = ",";
        }

        public static class Date
        {
            public static readonly CultureInfo BulgarianCultureInfo = new("bg-BG");
            public const string MonthNameDateFormat = "MMMM";
            public const string FullMonthNameAndYearFormat = "MMMM yyyy";
        }

        public static class Currency
        {
            public const decimal EuroToBgnExchangeRate = 1.95583M;
        }
        
        public const string SamsungGalaxyNineUserAgentValue = "Mozilla/5.0 (Linux; Android 8.0.0; SM-G960F Build/R16NW) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/62.0.3202.84 Mobile Safari/537.36";

        public static readonly KeyValuePair<string, string> UserAgentHeader = new("user-agent", SamsungGalaxyNineUserAgentValue);

        public static string SanitizeText(string input, params string[] stringsToSanitize)
        {
            if (input == null)
            {
                return null;
            }

            var builder = new StringBuilder(input);

            foreach (string stringToSanitize in stringsToSanitize)
            {
                builder.Replace(stringToSanitize, string.Empty);
            }

            return builder.ToString();
        }

        public static decimal GetCurrencyExchangeRate(string input)
        {
            if (input.Contains("eur"))
            {
                return Currency.EuroToBgnExchangeRate;
            }

            return 1;
        }

        public static async Task<string> GetHtmlAsync(string url, params KeyValuePair<string, string>[] headers)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, url);

            foreach (var header in headers)
            {
                request.Headers.Add(header.Key, header.Value);
            }

            var response = await client.SendAsync(request);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
