namespace MotoBest.Common
{
    public static class StringExtensions
    {
        public static string Capitalize(this string input)
        {
            return $"{input[0].ToString().ToUpper()}{input[1..]}";
        }
    }
}
