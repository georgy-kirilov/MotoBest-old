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

        public const string Eagle = "Eagle";
        public const string Excalibur = "Excalibur";

        public const string Fso = "FSO";
        public const string Ferrari = "Ferrari";
        public const string Fiat = "Fiat";
        public const string Ford = "Ford";
        public const string Foton = "Foton";

        public const string Gaz = "Gaz";
        public const string Geo = "Geo";
        public const string Gmc = "Gmc";
        public const string GreatWall = "Great Wall";
        public const string GacGonow = "Gac Gonow";

        public const string Haval = "Haval";
        public const string Heinkel = "Heinkel";
        public const string Hillman = "Hillman";
        public const string Honda = "Honda";
        public const string Hummer = "Hummer";
        public const string Hyundai = "Hyundai";
        public const string Humber = "Humber";

        public static IEnumerable<string> All()
        {
            return new[]
            {
                #region Brands with A
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
                #endregion
                #region Brands with B
                Bmw,
                Bentley,
                Berliner,
                Bertone,
                Borgward,
                Brilliance,
                Bugatti,
                Buick,
                #endregion
                #region Brands with C
                Cadillac,
                Chevrolet,
                Chrysler,
                Citroen,
                Corvette,
                Cupra,
                #endregion
                #region Brands with D
                DS,
                DR,
                Dacia,
                Daewoo,
                Daihatsu,
                Daimler,
                Datsun,
                Dkw,
                Dodge,
                #endregion
                #region Brands with E
                Eagle,
                Excalibur,
                #endregion
                #region Brands with F
                Fso,
                Ferrari,
                Fiat,
                Ford,
                Foton,
                #endregion
                #region Brands with G
                Gaz,
                Geo,
                Gmc,
                GreatWall,
                GacGonow,
                #endregion
                #region Brands with H
                Haval,
                Heinkel,
                Hillman,
                Honda,
                Hummer,
                Hyundai,
                Humber,
                #endregion
            };
        }
    }
}
