namespace MotoBest.Scraper
{
    using System;
    using System.Collections.Generic;

    using static MotoBest.Seeder.EngineSeeder;
    using static MotoBest.Seeder.ConditionSeeder;
    using static MotoBest.Seeder.TransmissionSeeder;
    using static MotoBest.Seeder.BodyStyleSeeder;
    using static MotoBest.Seeder.EuroStandardSeeder;

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

        private static readonly SortedDictionary<DateTime, string> EuroStandardsByDateTable = new()
        {
            { new DateTime(1992, 12, 31), EuroOne },
            { new DateTime(1997, 1, 1), EuroTwo },
            { new DateTime(2001, 1, 1), EuroThree },
            { new DateTime(2006, 1, 1), EuroFour },
            { new DateTime(2011, 1, 1), EuroFive },
            { new DateTime(2015, 9, 1), EuroSix },
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

        public static string EstimateEuroStandard(AdvertScrapeModel model)
        {
            model.IsEuroStandardExact = false;
            string currentEuroStandardType = null;

            foreach (var euroStandardDatePair in EuroStandardsByDateTable)
            {
                if (euroStandardDatePair.Key.CompareTo(model.ManufacturingDate) > 0)
                {
                    break;
                }

                currentEuroStandardType = euroStandardDatePair.Value;
            }

            return currentEuroStandardType;
        }
    }
}
