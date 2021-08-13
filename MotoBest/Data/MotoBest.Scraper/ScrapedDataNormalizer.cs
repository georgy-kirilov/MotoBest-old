﻿namespace MotoBest.Scraper
{
    using System.Collections.Generic;

    using static MotoBest.Seeder.EngineSeeder;
    using static MotoBest.Seeder.ConditionSeeder;
    using static MotoBest.Seeder.TransmissionSeeder;
    using static MotoBest.Seeder.BodyStyleSeeder;

    public class ScrapedDataNormalizer
    {
        private static readonly Dictionary<string, string> EngineVariations = new()
        {
            { Diesel, Diesel },
            { "дизел",  Diesel },

            { Gasoline, Gasoline },
            { "бензин", Gasoline },
            { "газ/бензин", Gasoline },
            { "метан/бензин", Gasoline },

            { Electric, Electric },
            { "електричество", Electric },
            
            { Hybrid, Hybrid },
            { "хибрид", Hybrid },
        };

        private static readonly Dictionary<string, string> ConditionVariations = new()
        {
            { New, New },
            { "нови", New },

            { Used, Used },
            { "употребявани", Used },

            { DamagedOrHit, DamagedOrHit },
            { "повредени/ударени", DamagedOrHit },

            { ForParts, ForParts },
        };

        private static readonly Dictionary<string, string> TransmissionVariations = new()
        {
            { Manual, Manual },
            { "ръчни", Manual },

            { Automatic, Automatic },
            { "автоматични", Automatic },

            { SemiAutomatic, SemiAutomatic },
        };

        private static readonly Dictionary<string, string> BodyStyleVariations = new()
        {
            { Van, Van },
            { Sedan, Sedan },
            { Hatchback, Hatchback },
            { StationWagon, StationWagon },
            { Coupe, Coupe },
            { Convertible, Convertible },
            { Jeep, Jeep },
            { Pickup, Pickup },
            { Minivan, Minivan },
            { StretchLimousine, StretchLimousine },
            { "лимузина", StretchLimousine },
            { BusOrMinibus, BusOrMinibus },
        };

        public static string NormalizeEngine(string engine)
        {
            if (engine == null)
            {
                return null;
            }

            string lowerEngine = engine.ToLower();
            return EngineVariations.ContainsKey(lowerEngine) ? EngineVariations[lowerEngine] : null;
        }

        public static string NormalizeCondition(string condition)
        {
            if (condition == null)
            {
                return null;
            }

            string lowerCondition = condition.ToLower();
            return ConditionVariations.ContainsKey(lowerCondition) ? ConditionVariations[lowerCondition] : null;
        }

        public static string NormalizeTransmission(string transmission)
        {
            if (transmission == null)
            {
                return null;
            }

            string lowerTransmission = transmission.ToLower();
            return TransmissionVariations.ContainsKey(lowerTransmission) ? TransmissionVariations[lowerTransmission] : null;
        }

        public static string NormalizeBodyStyle(string bodyStyle)
        {
            if (bodyStyle == null)
            {
                return null;
            }

            string lowerBodyStyle = bodyStyle.ToLower();
            return BodyStyleVariations.ContainsKey(lowerBodyStyle) ? BodyStyleVariations[lowerBodyStyle] : null;
        }
    }
}
