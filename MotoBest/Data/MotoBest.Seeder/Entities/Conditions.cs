namespace MotoBest.Seeding.Entities
{
    using System.Collections.Generic;

    public static class Conditions
    {
        public const string New = "нов";
        public const string Used = "употребяван";
        public const string DamagedOrHit = "повреден/ударен";
        public const string ForParts = "за части";

        public static IEnumerable<string> All()
        {
            return new[] { New, Used, DamagedOrHit, ForParts, };
        }
    }
}
