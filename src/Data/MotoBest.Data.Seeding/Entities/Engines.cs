namespace MotoBest.Data.Seeding.Entities
{
    using System.Collections.Generic;

    public static class Engines
    {
        public const string Gasoline = "бензинов";
        public const string Diesel = "дизелов";
        public const string Hybrid = "хибриден";
        public const string Electric = "електрически";

        public static IEnumerable<string> All()
        {
            return new[] { Gasoline, Diesel, Hybrid, Electric, };
        }
    }
}
