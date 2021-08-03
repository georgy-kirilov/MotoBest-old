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
    }
}
