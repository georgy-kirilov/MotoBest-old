﻿namespace MotoBest.Seeding.Entities
{
    using System.Collections.Generic;

    public static class EuroStandards
    {
        public const string EuroOne = "евро 1";
        public const string EuroTwo = "евро 2";
        public const string EuroThree = "евро 3";
        public const string EuroFour = "евро 4";
        public const string EuroFive = "евро 5";
        public const string EuroSix = "евро 6";

        public static IEnumerable<string> All()
        {
            return new[]
            {
                EuroOne,
                EuroTwo,
                EuroThree,
                EuroFour,
                EuroFive,
                EuroSix,
            };
        }
    }
}
