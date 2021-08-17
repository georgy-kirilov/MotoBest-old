namespace MotoBest.Common
{
    using System;

    public static class Validator
    {
        public static void ThrowIfNullOrEmpty(string param, string paramName)
        {
            if (string.IsNullOrEmpty(param))
            {
                throw new ArgumentException($"{paramName} cannot be null or empty");
            }
        }

        public static void ThrowIfNull(object obj, string paramName)
        {
            if (obj == null)
            {
                throw new ArgumentNullException($"{paramName} cannot be null");
            }
        }
    }
}
