namespace MotoBest.Seeding.Entities
{
    using System.Collections.Generic;

    public static class Transmissions
    {
        public const string Manual = "ръчна";
        public const string Automatic = "автоматична";
        public const string SemiAutomatic = "полуавтоматична";

        public static IEnumerable<string> All()
        {
            return new[] { Manual, Automatic, SemiAutomatic, };
        }
    }
}
