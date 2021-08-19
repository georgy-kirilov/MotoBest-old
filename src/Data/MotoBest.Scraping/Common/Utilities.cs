﻿namespace MotoBest.Scraping.Common
{
    using System.Text;
    using System.Globalization;
    using System.Threading.Tasks;
    using System.Net.Http;
    using System.Collections.Generic;

    public static class Utilities
    {
        public static readonly CultureInfo BulgarianCultureInfo = new("bg-BG");

        public const string MonthNameDateFormat = "MMMM";

        public const string Whitespace = " ";

        public const char NewLine = '\n';

        public const decimal EuroToBgnExchangeRate = 1.95583M;

        public const string SamsungGalaxyUserAgentValue = "Mozilla/5.0 (Linux; Android 8.0.0; SM-G960F Build/R16NW) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/62.0.3202.84 Mobile Safari/537.36";

        public static readonly KeyValuePair<string, string> UserAgentHeader = new("user-agent", SamsungGalaxyUserAgentValue);

        public static string SanitizeText(string input, params string[] stringsToSanitize)
        {
            var builder = new StringBuilder(input);

            foreach (string stringToSanitize in stringsToSanitize)
            {
                builder.Replace(stringToSanitize, string.Empty);
            }

            return builder.ToString();
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
