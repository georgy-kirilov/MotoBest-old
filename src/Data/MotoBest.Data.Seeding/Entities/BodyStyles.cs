namespace MotoBest.Data.Seeding.Entities
{
    using System.Collections.Generic;

    public static class BodyStyles
    {
        public const string Van = "ван";
        public const string Sedan = "седан";
        public const string Hatchback = "хечбек";
        public const string StationWagon = "комби";
        public const string Coupe = "купе";
        public const string Convertible = "кабрио";
        public const string Jeep = "джип";
        public const string Pickup = "пикап";
        public const string Minivan = "миниван";
        public const string StretchLimousine = "стреч лимузина";
        public const string BusOrMinibus = "бус/минибус";

        public static IEnumerable<string> All()
        {
            return new[]
            {
                Van,
                Sedan,
                Hatchback,
                StationWagon,
                Coupe,
                Convertible,
                Jeep,
                Pickup,
                Minivan,
                StretchLimousine,
                BusOrMinibus,
            };
        }
    }
}
