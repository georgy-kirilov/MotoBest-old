namespace MotoBest.Seeding.Entities
{
    using System.Collections.Generic;

    public static class Brands
    {
        public const string Abarth = "Abarth";
        public const string Acura = "Acura";
        public const string Aixam = "Aixam";
        public const string AlfaRomeo = "Alfa Romeo";
        public const string Alpina = "Alpina";
        public const string Aro = "Aro";
        public const string AsiaMotors = "Asia Motors";
        public const string AstonMartin = "Aston Martin";
        public const string Audi = "Audi";
        public const string Austin = "Austin";

        public const string Bmw = "BMW";
        public const string Bentley = "Bentley";
        public const string Berliner = "Berliner";
        public const string Bertone = "Bertone";
        public const string Borgward = "Borgward";
        public const string Brilliance = "Brilliance";
        public const string Bugatti = "Bugatti";
        public const string Buick = "Buick";

        public const string Cadillac = "Cadillac";
        public const string Chevrolet = "Chevrolet";
        public const string Chrysler = "Chrysler";
        public const string Citroen = "Citroen";
        public const string Corvette = "Corvette";
        public const string Cupra = "Cupra";

        public const string DS = "DS";
        public const string DR = "DR";
        public const string Dacia = "Dacia";
        public const string Daewoo = "Daewoo";
        public const string Daihatsu = "Daihatsu";
        public const string Daimler = "Daimler";
        public const string Datsun = "Datsun";
        public const string Dkw = "Dkw";
        public const string Dodge = "Dodge";

        public static IEnumerable<string> All()
        {
            return new[]
            {
                Abarth,
                Acura,
                Aixam,
                AlfaRomeo,
                Alpina,
                Aro,
                AsiaMotors,
                AstonMartin,
                Audi,
                Austin,

                Bmw,
                Bentley,
                Berliner,
                Bertone,
                Borgward,
                Brilliance,
                Bugatti,
                Buick,

                Cadillac,
                Chevrolet,
                Chrysler,
                Citroen,
                Corvette,
                Cupra,

                DS,
                DR,
                Dacia,
                Daewoo,
                Daihatsu,
                Daimler,
                Datsun,
                Dkw,
                Dodge,
            };
        }
    }
}
