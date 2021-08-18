﻿namespace MotoBest.Scraping.Common
{
    using System;
    using System.Collections.Generic;

    using MotoBest.Seeding.Entities;

    using static MotoBest.Seeding.Entities.Engines;
    using static MotoBest.Seeding.Entities.Conditions;
    using static MotoBest.Seeding.Entities.Transmissions;
    using static MotoBest.Seeding.Entities.BodyStyles;
    using static MotoBest.Seeding.Entities.EuroStandards;
    using static MotoBest.Seeding.Entities.Colors;
    using static MotoBest.Seeding.Entities.Regions;

    public static class ScrapedDataNormalizer
    {
        private static readonly Dictionary<string, string> EngineVariations = new();
        private static readonly Dictionary<string, string> ConditionVariations = new();
        private static readonly Dictionary<string, string> TransmissionVariations = new();
        private static readonly Dictionary<string, string> BodyStyleVariations = new();
        private static readonly Dictionary<string, string> ColorVariations = new();
        private static readonly Dictionary<string, string> RegionVariations = new();
        private static readonly SortedDictionary<DateTime, string> EuroStandardsByDateTable;

        static ScrapedDataNormalizer()
        {
            InitializeEngineVariations();
            InitializeConditionVariations();
            InitializeTransmissionVariations();
            InitializeBodyStyleVariations();
            InitializeColorVariations();
            InitializeRegionVariations();

            EuroStandardsByDateTable = new()
            {
                { new DateTime(1992, 12, 31), EuroOne },
                { new DateTime(1997, 1, 1), EuroTwo },
                { new DateTime(2001, 1, 1), EuroThree },
                { new DateTime(2006, 1, 1), EuroFour },
                { new DateTime(2011, 1, 1), EuroFive },
                { new DateTime(2015, 9, 1), EuroSix },
            };
        }

        private static void InitializeEngineVariations()
        {
            InitializeVariationsTable(EngineVariations, Engines.All());
            EngineVariations.Add("дизел", Diesel);
            EngineVariations.Add("бензин", Gasoline);
            EngineVariations.Add("газ/бензин", Gasoline);
            EngineVariations.Add("метан/бензин", Gasoline);
            EngineVariations.Add("електричество", Electric);
            EngineVariations.Add("хибрид", Hybrid);
        }

        private static void InitializeConditionVariations()
        {
            InitializeVariationsTable(ConditionVariations, Conditions.All());
            ConditionVariations.Add("нови", New);
            ConditionVariations.Add("употребявани", Used);
            ConditionVariations.Add("повредени/ударени", DamagedOrHit);
        }

        private static void InitializeTransmissionVariations()
        {
            InitializeVariationsTable(TransmissionVariations, Transmissions.All());
            TransmissionVariations.Add("ръчни", Manual);
            TransmissionVariations.Add("автоматични", Automatic);
        }

        private static void InitializeBodyStyleVariations()
        {
            InitializeVariationsTable(BodyStyleVariations, BodyStyles.All());
            BodyStyleVariations.Add("лимузина", StretchLimousine);
        }

        private static void InitializeColorVariations()
        {
            InitializeVariationsTable(ColorVariations, Colors.All());
            ColorVariations.Add("тъмно син мет.", DarkBlue);
            ColorVariations.Add("tъмно син", DarkBlue);
            ColorVariations.Add("т.зелен", DarkGreen);
            ColorVariations.Add("бронз", Bronze);
            ColorVariations.Add("златен", Gold);
        }

        private static void InitializeRegionVariations()
        {
            InitializeVariationsTable(RegionVariations, Regions.All());
            RegionVariations.Add("Дупница", Kyustendil);
            RegionVariations.Add("София - град", Sofia);
            RegionVariations.Add("Софийска област", Sofia);
        }

        private static void InitializeVariationsTable(Dictionary<string, string> table, IEnumerable<string> values)
        {
            foreach (string value in values)
            {
                table.Add(value, value);
            }
        }

        public static string NormalizeEngine(string engine)
        {
            return NormalizeEntity(engine, EngineVariations);
        }

        public static string NormalizeCondition(string condition)
        {
            return NormalizeEntity(condition, ConditionVariations);
        }

        public static string NormalizeTransmission(string transmission)
        {
            return NormalizeEntity(transmission, TransmissionVariations);
        }

        public static string NormalizeBodyStyle(string bodyStyle)
        {
            return NormalizeEntity(bodyStyle, BodyStyleVariations);
        }

        public static string NormalizeColor(string color)
        {
            return NormalizeEntity(color, ColorVariations);
        }

        public static string NormalizeRegion(string region)
        {
            if (region == null)
            {
                return null;
            }

            return RegionVariations.ContainsKey(region) ? RegionVariations[region] : null;
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

        private static string NormalizeEntity(string entity, IDictionary<string, string> entityVariations)
        {
            if (entity == null)
            {
                return null;
            }

            string lowerEntity = entity.ToLower();
            return entityVariations.ContainsKey(lowerEntity) ? entityVariations[lowerEntity] : null;
        }
    }
}